using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Windows;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// User Settings, such as window location and size.
    /// </summary>
    public class UserSettings
    {
        private static string filename;
        private const int MAX_FOLDERS = 15;

        #region Constructor

        public UserSettings() {
            this.RecentlyUsedFolders = [];
        }

        #endregion

        #region Properties

        public double WindowX { get; set; }
        public double WindowY { get; set; }

        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }

        public WindowState WindowState { get; set; }

        public double FontSize { get; set; }

        public string Theme { get; set; }

        public List<string> RecentlyUsedFolders { get; set; }

        public bool GenerateFlowEvents { get; set; }

        public bool ClearFilesBeforeAddingMore { get; set; }


        public int ProgressInterval { get; set; }


        /// <summary>
        /// Gets the fully qualified name of the settings file.
        /// </summary>
        public static string Filename
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(UserSettings.filename))
                {
                    return UserSettings.filename;
                }

                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appName = Assembly.GetExecutingAssembly().GetName().Name;
                string settingsFilePath = Path.Combine(appDataPath, appName, "UserSettings.json");

                return UserSettings.filename = settingsFilePath;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a recently used folder, and evicts an old folder if necessary
        /// to keep the list size within reason.
        /// </summary>
        /// <param name="folder">The folder to add to the list.</param>
        public void AddFolder(string folder)
        {
            if (this.RecentlyUsedFolders.Contains(folder))
            {
                return;
            }

            if (this.RecentlyUsedFolders.Count >= UserSettings.MAX_FOLDERS)
            {
                this.RecentlyUsedFolders.RemoveAt(0);
            }

            this.RecentlyUsedFolders.Add(folder);
        }

        /// <summary>
        /// Saves the <see cref="UserSettings"/> instance to <see cref="Filename"/>.
        /// </summary>
        public void Save()
        {
            try
            {
                UserSettings.CreateApplicationDataSubFolder();

                var json = JsonConvert.SerializeObject(this);
                File.WriteAllText(UserSettings.Filename, json);
            }
            catch (Exception)
            {
                // Swallow for now
            }
        }

        /// <summary>
        /// Loads a <see cref="UserSettings"/> instance from <see cref="Filename"/>.
        /// </summary>
        /// <returns></returns>
        public static UserSettings Load()
        {
            try
            {
                UserSettings.CreateApplicationDataSubFolder();

                if (File.Exists(UserSettings.Filename))
                {
                    var json = File.ReadAllText(UserSettings.Filename);
                    var settings = JsonConvert.DeserializeObject<UserSettings>(json);

                    settings.EnforceLimits();

                    return settings;
                }
            }
            catch(Exception)
            {
                // Swallow for now
            }

            return UserSettings.CreateWithDefaults();
        }

        public static UserSettings CreateWithDefaults()
        {
            return new() {
                GenerateFlowEvents = true,
                ProgressInterval = 500,
            };
        }

        internal void EnforceLimits()
        {
            if (this.ProgressInterval < 100)
            {
                this.ProgressInterval = 100;
            }

            if (this.ProgressInterval > 10000)
            {
                this.ProgressInterval = 10000;
            }

            this.RecentlyUsedFolders ??= [];
        }

        /// <summary>
        /// Creates an application specific folder under AppData, if one
        /// doesn't already exist.
        /// </summary>
        internal static void CreateApplicationDataSubFolder()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appName = Assembly.GetExecutingAssembly().GetName().Name;
            string appSubFolder = Path.Combine(appDataPath, appName);

            if (!Directory.Exists(appSubFolder))
            {
                Directory.CreateDirectory(appSubFolder);
            }
        }

        #endregion
    }
}
