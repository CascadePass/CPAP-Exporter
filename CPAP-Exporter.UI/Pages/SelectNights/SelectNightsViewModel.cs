using cpaplib;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// ViewModel for selecting nights from a DataGrid.
    /// </summary>
    public class SelectNightsViewModel : PageViewModel
    {
        #region Private fields

        private DailyReportViewModel selectedReport;
        private bool isAllSelected, clearReportsBeforeAdding;
        private DelegateCommand openSourceFolderCommand;

        private static Dictionary<string, int> loadedFolders;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectNightsViewModel"/> class.
        /// </summary>
        public SelectNightsViewModel() : base(Resources.PageTitle_SelectNights, Resources.PageDesc_SelectNights)
        {
        }

        public SelectNightsViewModel(ExportParameters exportParameters) : this()
        {
            this.ExportParameters = exportParameters;
            this.ClearReportsBeforeAdding = exportParameters.UserPreferences.ClearFilesBeforeAddingMore;

            if (!string.IsNullOrWhiteSpace(this.ExportParameters?.SourcePath))
            {
                this.Work();
            }
            else
            {
                this.ShowDefaultStatusMessage();
            }
        }

        static SelectNightsViewModel()
        {
            SelectNightsViewModel.loadedFolders = new();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether all reports are selected.
        /// </summary>
        public bool IsAllSelected
        {
            get => this.isAllSelected;
            set
            {
                if (this.SetPropertyValue(ref this.isAllSelected, value, nameof(this.IsAllSelected)))
                {
                    foreach (var report in this.Reports)
                    {
                        report.IsSelected = value;
                    }
                }
            }
        }

        public bool ClearReportsBeforeAdding
        {
            get => this.clearReportsBeforeAdding;
            set => this.SetPropertyValue(ref this.clearReportsBeforeAdding, value, nameof(this.ClearReportsBeforeAdding));
        }

        /// <summary>
        /// Gets or sets the collection of daily reports.
        /// </summary>
        public ObservableCollection<DailyReportViewModel> Reports => this.ExportParameters.Reports;

        /// <summary>
        /// Gets or sets the selected report.
        /// </summary>
        public DailyReportViewModel SelectedReport
        {
            get => this.selectedReport;
            set => this.SetPropertyValue(ref this.selectedReport, value, nameof(this.SelectedReport));
        }

        public List<KeyValuePair<string, int>> SourceFolders => [..SelectNightsViewModel.loadedFolders];

        /// <summary>
        /// Gets the command to open the source folder.
        /// </summary>
        public ICommand OpenSourceFolderCommand => this.openSourceFolderCommand ??= new DelegateCommand(this.BrowseToSourceFolder);

        #endregion

        #region Methods

        /// <summary>
        /// Loads the reports from the specified folder.
        /// </summary>
        /// <param name="folder">The folder to load reports from.</param>
        /// <param name="replaceExisting">If set to <c>true</c>, replaces existing reports.</param>
        public void LoadFromFolder(string folder, bool replaceExisting)
        {
            if (SelectNightsViewModel.loadedFolders.ContainsKey(folder))
            {
                return;
            }

            #region Get ICpapDataLoader or quit

            if (!Path.Exists(folder))
            {
                // Any existing reports have not been cleared.
                return;
            }

            ICpapDataLoader loader = new ResMedDataLoader();

            if (!loader.HasCorrectFolderStructure(folder))
            {
                loader = new PRS1DataLoader();

                if (!loader.HasCorrectFolderStructure(folder))
                {
                    // Any existing reports have not been cleared.
                    return;
                }
            }

            #endregion

            #region Clear existing (if desired)

            // The fact that we're here means that the folder exists and the loader is set.

            if (replaceExisting)
            {
                this.Reports.Clear();
                SelectNightsViewModel.loadedFolders.Clear();
            }

            #endregion

            this.IsBusy = true;
            this.ShowBusyStatus();
            this.ExportParameters.SourcePath = folder;
            ApplicationComponentProvider.Status.StatusText = string.Format(Resources.ReadingFolder, folder);

            // Prepare to load reports
            List<DailyReport> reports = null;

            try
            {
                // This is where the files are loaded from disc
                reports = loader.LoadFromFolder(folder, null, null, new() { FlagFlowLimits = this.ExportParameters.UserPreferences.GenerateFlowEvents });
                this.StatusContent = null;
            }
            catch (Exception ex)
            {
                ApplicationComponentProvider.Status.StatusText += ex.ToString();
            }

            // And now it's time to process them.
            if (reports.Count > 0)
            {
                if (Dispatcher.CurrentDispatcher != null && !Dispatcher.CurrentDispatcher.CheckAccess())
                {
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        this.ConsumeReports(reports);
                    });
                }
                else
                {
                    this.ConsumeReports(reports);
                }
            }

            if (!this.IsAllSelected && reports.Count > 0)
            {
                this.ExportParameters.Reports[^1].IsSelected = true;
                this.IsAllSelected = this.ExportParameters.Reports.All(r => r.IsSelected);
            }

            if (!SelectNightsViewModel.loadedFolders.ContainsKey(folder))
            {
                SelectNightsViewModel.loadedFolders.Add(folder, reports.Count);
            }

            this.OnPropertyChanged(nameof(this.SourceFolders));
            this.IsBusy = false;
            this.ShowDefaultStatusMessage();
        }

        internal void ConsumeReports(List<DailyReport> reports)
        {
            ApplicationComponentProvider.Status.ProgressBar = new(0, reports.Count - 1, 0);

            for (int i = 0; i < reports.Count; i++)
            {
                ApplicationComponentProvider.Status.ProgressBar.Current = i;

                var report = reports[i];
                //bool isDetailReport = report.Sessions.Count > 0 && !report.Sessions.Any(session => session.Signals.Count == 0);

                if (report.HasDetailData)
                {
                    this.AddReport(report);
                }
            }

            ApplicationComponentProvider.Status.ProgressBar = null;
        }

        internal DailyReportViewModel AddReport(DailyReport report)
        {
            ArgumentNullException.ThrowIfNull(report, nameof(report));

            ApplicationComponentProvider.Status.StatusText = string.Format(Resources.AddingDate, report.ReportDate);

            DailyReportViewModel reportViewModel = new(report, this.IsAllSelected);
            this.Reports.Add(reportViewModel);

            reportViewModel.PropertyChanged += this.ReportViewModel_PropertyChanged;

            return reportViewModel;
        }

        /// <summary>
        /// Opens Windows Explorder the source folder in the file explorer.
        /// </summary>
        public void BrowseToSourceFolder()
        {
            WindowsExplorerUtility.BrowseToFolder(this.ExportParameters.SourcePath);
        }

        public override bool Validate()
        {
            return this.Reports?.Any(report => report.IsSelected) ?? false;
        }

        public void Work()
        {
            if (SelectNightsViewModel.loadedFolders.ContainsKey(this.ExportParameters.SourcePath))
            {
                this.ShowDefaultStatusMessage();
                return;
            }

            ApplicationComponentProvider.Status.StatusText = Resources.Working;

            Task.Run(() =>
            {
                this.LoadFromFolder(this.ExportParameters.SourcePath, this.ClearReportsBeforeAdding);
            });
        }

        internal string GetFolderList()
        {
            return string.Join(',', SelectNightsViewModel.loadedFolders.Keys);
        }

        internal void ShowDefaultStatusMessage()
        {
            if (Application.Current is null)
            {
                // Unit tests are running, or I'm in some unnatural context where the application
                // hasn't been initialized.

                this.StatusContent = string.Format(
                    Resources.ReportsSelected,
                    this.ExportParameters.Reports.Count(report => report.IsSelected),
                    this.ExportParameters.Reports.Count
                );

                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                var selectedReportCount = this.ExportParameters.Reports.Count(report => report.IsSelected);

                if (selectedReportCount == 0)
                {
                    this.StatusContent = new WarningCueElement(
                        Resources.Validation_NoNightsSelected
                    );
                }
                else
                {
                    this.StatusContent = new InfoCueElement(
                        string.Format(
                            Resources.ReportsSelected,
                            selectedReportCount,
                            this.ExportParameters.Reports.Count
                        ))
                    {
                        DisplayDuration = TimeSpan.FromSeconds(8),
                        IconSource = new BitmapImage(new Uri("pack://application:,,,/Images/CPAP-Exporter.VersionEmblem.1.1.0.png")),
                    };
                }
            });
        }

        internal void ShowBusyStatus()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.StatusContent = new BusyCueElement();
            });
        }

        private void ReportViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Reports));
            this.ShowDefaultStatusMessage();
        }


        #endregion
    }
}