using Microsoft.Win32;
using System.Windows;

namespace CascadePass.CPAPExporter
{
    public abstract class ThemeDetector : IThemeDetector, IDisposable
    {
        private readonly IRegistryProvider registryProvider;

        public event EventHandler ThemeChanged;

        #region Constructors

        public ThemeDetector()
        {
            this.registryProvider = new RegistryProvider();
            SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
            this.FollowSystemTheme = true;
        }

        public ThemeDetector(IRegistryProvider registryProviderToUse)
        {
            this.registryProvider = registryProviderToUse;
            SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
            this.FollowSystemTheme = true;
        }

        #endregion

        #region Properties

        public bool FollowSystemTheme { get; set; }

        public IRegistryProvider RegistryProvider => this.registryProvider;

        public bool IsInLightMode =>
                    this.RegistryProvider.GetValue(
                        @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                        "AppsUseLightTheme"
                        )
                    ?.Equals(1) ?? true;

        public bool IsHighContrastEnabled => SystemParameters.HighContrast;

        #endregion

        #region Methods

        public void Dispose()
        {
            SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
        }

        public ThemeType GetThemeType()
        {
            if (this.IsHighContrastEnabled)
            {
                return ThemeType.HighContrast;
            }

            if (this.IsInLightMode)
            {
                return ThemeType.Light;
            }

            return ThemeType.Dark;
        }

        public abstract bool ApplyTheme();

        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                this.OnThemeChanged(sender, e);
            }
        }

        protected async void OnThemeChanged(object sender, EventArgs e)
        {
            if (this.FollowSystemTheme)
            {
                await Task.Delay(50);
                this.ApplyTheme();
                this.ThemeChanged?.Invoke(this, e);
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a type or category of theme, of which there can be many individual
    /// themes.  Applications typically implement their own themes, sometimes many;
    /// ThemeType describes the categories these should fall into.
    /// </summary>
    public enum ThemeType
    {
        /// <summary>
        /// Value has not ben set.
        /// </summary>
        None,

        /// <summary>
        /// Light mode.
        /// </summary>
        Light,

        /// <summary>
        /// Dark mode.
        /// </summary>
        Dark,

        /// <summary>
        /// High contrast mode, often used with other accessibility features.
        /// </summary>
        HighContrast
    }

    public interface IRegistryProvider
    {
        event RegistryProvider.RegistryAccessedHandler RegistryAccessed;
        event RegistryProvider.RegistryAccessedAsyncHandler RegistryAccessedAsync;

        bool DeleteValue(string keyName, string valueName);
        string[] GetSubKeyNames(string keyName);
        object GetValue(string keyName, string valueName);
        string[] GetValueNames(string keyName);
        bool SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind);
    }

    public class RegistryProvider : IRegistryProvider
    {
        private readonly RegistryHive hive;

        public delegate void RegistryAccessedHandler(object sender, RegistryAccessEventArgs e);
        public delegate Task RegistryAccessedAsyncHandler(object sender, RegistryAccessEventArgs e);

        public event RegistryAccessedHandler RegistryAccessed;
        public event RegistryAccessedAsyncHandler RegistryAccessedAsync;

        #region Constructors

        public RegistryProvider()
        {
            this.hive = RegistryHive.CurrentUser;
        }

        public RegistryProvider(RegistryHive targetHive)
        {
            this.hive = targetHive;
        }

        #endregion

        private RegistryKey OpenKey(string keyName, bool writable)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            return RegistryKey.OpenBaseKey(hive, RegistryView.Default).OpenSubKey(keyName, writable);
        }

        public object GetValue(string keyName, string valueName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            try
            {
                using var key = this.OpenKey(keyName, writable: false);
                object value = key?.GetValue(valueName);

                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Read));
                return value;
            }
            catch (Exception ex)
            {
                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Read, ex));
                return null;
            }
        }

        public bool SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            try
            {
                using var key = RegistryKey.OpenBaseKey(hive, RegistryView.Default).CreateSubKey(keyName);
                key?.SetValue(valueName, value, valueKind);

                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Write));
                return true;
            }
            catch (Exception ex)
            {
                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Write, ex));
                return false;
            }
        }

        public bool DeleteValue(string keyName, string valueName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            try
            {
                using var key = this.OpenKey(keyName, writable: true);
                key?.DeleteValue(valueName, throwOnMissingValue: false);

                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Delete));
                return true;
            }
            catch (Exception ex)
            {
                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Delete, ex));
                return false;
            }
        }

        public string[] GetSubKeyNames(string keyName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            try
            {
                using var key = this.OpenKey(keyName, writable: false);
                var result = key?.GetSubKeyNames() ?? [];

                this.OnRegistryAccessed(new(this.hive, keyName, RegistryAccessType.EnumerateKeys));
                return result;
            }
            catch (Exception ex)
            {
                this.OnRegistryAccessed(new(this.hive, keyName, RegistryAccessType.EnumerateKeys, ex));
                return null;
            }
        }

        public string[] GetValueNames(string keyName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            try
            {
                using var key = this.OpenKey(keyName, writable: false);
                var result = key?.GetValueNames() ?? [];

                this.OnRegistryAccessed(new(this.hive, keyName, RegistryAccessType.EnumerateValues));
                return result;
            }
            catch (Exception ex)
            {
                this.OnRegistryAccessed(new(this.hive, keyName, RegistryAccessType.EnumerateValues, ex));
                return null;
            }
        }

        protected virtual void OnRegistryAccessed(RegistryAccessEventArgs e)
        {
            this.RegistryAccessed?.Invoke(this, e);
            this.OnRegistryAccessedAsync(e);
        }

        protected virtual void OnRegistryAccessedAsync(RegistryAccessEventArgs e)
        {
            if (this.RegistryAccessedAsync != null)
            {
                var invocationList = this.RegistryAccessedAsync.GetInvocationList().Cast<RegistryAccessedAsyncHandler>();
                foreach (var handler in invocationList)
                {
                    _ = handler(this, e); // Fire-and-forget async
                }
            }
        }
    }

    /// <summary>
    /// Provides data for events related to Windows Registry access operations, 
    /// including key and value names, access type, outcome, and any associated exception.
    /// </summary>
    public class RegistryAccessEventArgs : EventArgs
    {
        #region Constructors

        internal RegistryAccessEventArgs(
            RegistryHive hive,
            string keyName,
            RegistryAccessType accessType)
        {
            this.Hive = hive;
            this.KeyName = keyName;
            this.AccessType = accessType;
        }

        internal RegistryAccessEventArgs(
            RegistryHive hive,
            string keyName,
            string valueName,
            RegistryAccessType accessType)
        {
            this.Hive = hive;
            this.KeyName = keyName;
            this.ValueName = valueName;
            this.AccessType = accessType;
        }

        internal RegistryAccessEventArgs(
            RegistryHive hive,
            string keyName,
            RegistryAccessType accessType,
            Exception exception)
        {
            this.Hive = hive;
            this.KeyName = keyName;
            this.AccessType = accessType;
            this.Exception = exception;
        }

        internal RegistryAccessEventArgs(
            RegistryHive hive,
            string keyName,
            string valueName,
            RegistryAccessType accessType,
            Exception exception)
        {
            this.Hive = hive;
            this.KeyName = keyName;
            this.ValueName = valueName;
            this.AccessType = accessType;
            this.Exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the registry hive involved in the operation.
        /// </summary>
        public RegistryHive Hive { get; }

        /// <summary>
        /// Gets the name of the registry key involved in the operation.
        /// </summary>
        public string KeyName { get; }

        /// <summary>
        /// Gets the name of the value within the registry key that was accessed or modified.
        /// </summary>
        public string ValueName { get; }

        /// <summary>
        /// Gets the type of registry access operation that was performed.
        /// </summary>
        public RegistryAccessType AccessType { get; }

        /// <summary>
        /// Gets a value indicating whether the registry operation completed without throwing an exception.
        /// </summary>
        public bool WasSuccessful => this.Exception == null;

        /// <summary>
        /// Gets the exception thrown during the registry operation, if any. 
        /// Returns <c>null</c> if the operation was successful.
        /// </summary>
        public Exception Exception { get; }

        #endregion
    }

    public enum RegistryAccessType
    {
        None,

        Read,

        Write,

        Delete,

        EnumerateKeys,

        EnumerateValues,
    }

    public interface IThemeDetector
    {
        bool FollowSystemTheme { get; set; }
        bool IsHighContrastEnabled { get; }
        bool IsInLightMode { get; }
        IRegistryProvider RegistryProvider { get; }

        event EventHandler ThemeChanged;

        bool ApplyTheme();
        void Dispose();
        ThemeType GetThemeType();
    }
}
