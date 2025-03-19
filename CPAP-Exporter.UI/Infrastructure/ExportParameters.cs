using CascadePass.CPAPExporter.Core;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace CascadePass.CPAPExporter
{
    public class ExportParameters : Observable
    {
        #region Fields

        private ObservableCollection<ExportSettings> settings;
        private ObservableCollection<DailyReportViewModel> reports;
        private ObservableCollection<SignalViewModel> signals;
        private List<string> signalNames;
        private string sourcePath, destinationPath;
        private UserSettings userSettings;

        #endregion

        #region Constructor

        public ExportParameters()
        {
            // Create collection instances
            
            this.Reports = [];
            this.Signals = [];
            this.Settings = [];

            // Tell WPF to manage concurrency for these collections.  This
            // avoids the need for manual thread synchronization.  It's not
            // as efficient, but this application won't put enough data into
            // these collections for it to matter.

            BindingOperations.EnableCollectionSynchronization(this.Reports, new object());
            BindingOperations.EnableCollectionSynchronization(this.Signals, new object());
            BindingOperations.EnableCollectionSynchronization(this.Settings, new object());

            // Get the user's settings, if available.

            this.UserPreferences = UserSettings.Load();
        }

        #endregion

        #region Properties

        #region UI Facing

        /// <summary>
        /// Gets or sets the <see cref="DailyReport"/> objects to export, wrapped
        /// in <see cref="DailyReportViewModel"/> objects with an <see cref="DailyReportViewModel.IsSelected"/>
        /// property to identify which nights to export."/>
        /// </summary>
        public ObservableCollection<DailyReportViewModel> Reports
        {
            get => this.reports;
            set => this.SetPropertyValue(ref this.reports, value, nameof(this.Reports));
        }

        /// <summary>
        /// Gets or sets the <see cref="Signal"/> objects to export, wrapped
        /// in <see cref="SignalViewModel"/> objects with <see cref="SignalViewModel.IsSelected"/>
        /// properties identifying which to export."/>
        /// </summary>
        /// <remarks>
        /// Columns in the CSV output will be ordered according to the order of this property.
        /// </remarks>
        public ObservableCollection<SignalViewModel> Signals
        {
            get => this.signals;
            set
            {
                if (this.SetPropertyValue(ref this.signals, value, [nameof(this.Signals), nameof(this.SignalNames)]))
                {
                    this.signalNames = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of settings for the exports.
        /// </summary>
        /// <remarks>
        /// The user will be able to export to CSV and Xml formats,
        /// each having its own settings.
        /// </remarks>
        public ObservableCollection<ExportSettings> Settings
        {
            get => this.settings;
            set => this.SetPropertyValue(ref this.settings, value, nameof(this.Settings));
        }

        public UserSettings UserPreferences
        {
            get => this.userSettings;
            set => this.SetPropertyValue(ref this.userSettings, value, nameof(this.UserPreferences));
        }

        #endregion

        /// <summary>
        /// Gets the names of the selected signals as strings.
        /// </summary>
        public List<string> SignalNames => this.signalNames ??= [.. this.Signals.Where(s => s.IsSelected).Select(s => s.SignalInfo.Name)];

        public string SourcePath
        {
            get => this.sourcePath;
            set => this.SetPropertyValue(ref this.sourcePath, value, nameof(this.SourcePath));
        }

        public string DestinationPath
        {
            get => this.destinationPath;
            set => this.SetPropertyValue(ref this.destinationPath, value, nameof(this.DestinationPath));
        }

        #endregion
    }
}
