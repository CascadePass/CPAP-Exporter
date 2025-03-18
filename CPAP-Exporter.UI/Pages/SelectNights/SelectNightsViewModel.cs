using cpaplib;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
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
        private bool isAllSelected;
        private DelegateCommand openSourceFolderCommand;

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

            if (!string.IsNullOrWhiteSpace(this.ExportParameters?.SourcePath) && this.ExportParameters.Reports.Count == 0)
            {
                this.Work();
            }
            else
            {
                ApplicationComponentProvider.Status.StatusText = string.Format(Resources.NightsAvailable, this.ExportParameters.Reports.Count, this.ExportParameters?.SourcePath);
            }
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
            }

            #endregion

            this.IsBusy = true;
            ApplicationComponentProvider.Status.StatusText = string.Format(Resources.ReadingFolder, folder);

            // This is where the files are loaded from disc
            var reports = loader.LoadFromFolder(folder, null, null, new() { FlagFlowLimits = false });

            // And now it's time to process them.
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                foreach (var report in reports)
                {
                    ApplicationComponentProvider.Status.StatusText = string.Format(Resources.AddingDate, report.ReportDate);

                    DailyReportViewModel reportViewModel = new(report, false);
                    this.Reports.Add(reportViewModel);

                    reportViewModel.PropertyChanged += this.ReportViewModel_PropertyChanged;
                }
            });

            this.ExportParameters.Reports[^1].IsSelected = true;
            this.IsAllSelected = this.ExportParameters.Reports.All(r => r.IsSelected);

            this.IsBusy = false;
            ApplicationComponentProvider.Status.StatusText = string.Format(Resources.NightsAvailable, this.ExportParameters.Reports.Count, folder);
            this.ExportParameters.SourcePath = folder;
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
            ApplicationComponentProvider.Status.StatusText = Resources.Working;

            Task.Run(() =>
            {
                this.LoadFromFolder(this.ExportParameters.SourcePath, true);
            });
        }


        private void ReportViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Reports));
        }


        #endregion
    }
}