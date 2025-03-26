using CascadePass.CPAPExporter.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CascadePass.CPAPExporter
{
    public class SavedFilesViewModel : PageViewModel
    {
        private DelegateCommand browseFolderCommand;

        #region Constructor

        public SavedFilesViewModel() : base(Resources.PageTitle_SavedFiles, Resources.PageDesc_SavedFiles)
        {
            this.Files = [];

            // Assign the application's Dispatcher to allow the ViewModel to marshal updates to the UI thread. 
            // Application.Current may be null during unit testing or in non-UI contexts.

            this.Dispatcher = Application.Current?.Dispatcher;

            if (this.Dispatcher == null && Application.Current != null)
            {
                throw new InvalidOperationException();
            }
        }

        #endregion

        #region Properties

        public ObservableCollection<SavedFileViewModel> Files { get; set; }

        public Dispatcher Dispatcher { get; set; }

        public ICommand BrowseFolderCommand => this.browseFolderCommand ??= new DelegateCommand(this.BrowseFolder);

        #endregion

        #region Methods

        public SavedFileViewModel AddFile(string filename, string fileDescription)
        {
            if (this.Dispatcher?.CheckAccess() ?? true)
            {
                // Already on the UI thread, or there is no dispatcher meaning this is a unit test.
                return AddFileInternal(filename, fileDescription);
            }
            else
            {
                // On a background thread, so marshal the call to the UI thread
                return this.Dispatcher.Invoke(() => AddFileInternal(filename, fileDescription));
            }
        }

        private SavedFileViewModel AddFileInternal(string filename, string fileDescription)
        {
            if (File.Exists(filename))
            {
                SavedFileViewModel fileViewModel = new(filename, fileDescription);
                this.Files.Add(fileViewModel);

                fileViewModel.PropertyChanged += this.FileViewModel_PropertyChanged;
                fileViewModel.FileDeleted += this.FileViewModel_FileDeleted;

                return fileViewModel;
            }

            return null;
        }

        public void PerformExportAsync(string folder)
        {
            Task.Run(() =>
            {
                this.PerformExport(folder);
            });
        }

        public void PerformExport(string folder)
        {
            ApplicationComponentProvider.Status.StatusText = Resources.Working;

            try
            {
                CsvExportSettings csvSettings = (CsvExportSettings)this.ExportParameters.Settings.FirstOrDefault(s => s is CsvExportSettings) ?? new();
                CsvExporter exporter = new(
                    [.. this.ExportParameters.Reports.Where(r => r.IsSelected).Select(r => r.DailyReport)],
                    this.ExportParameters.SignalNames
                    );

                csvSettings.ProgressInterval = Math.Min(100, Math.Max(10000, csvSettings.ProgressInterval));

                exporter.Progress += this.Exporter_Progress;

                if (csvSettings.OutputFileHandling == OutputFileRule.CombinedIntoSingleFile)
                {
                    exporter.ExportToFile(Path.Combine(folder, csvSettings.Filenames.First()));
                    exporter.ExportFlaggedEventsToFile(Path.Combine(folder, csvSettings.EventFilenames.First()));

                    this.AddFile(Path.Combine(folder, csvSettings.Filenames.First()), Resources.FilesLabel_FullExport);
                    this.AddFile(Path.Combine(folder, csvSettings.EventFilenames.First()), Resources.FilesLabel_EventsExport);
                }
                else
                {
                    exporter.DailyReports.Clear();
                    var relevantDailyReports = this.ExportParameters.Reports.Where(r => r.IsSelected);

                    for (int i = 0; i < relevantDailyReports.Count(); i++)
                    {
                        exporter.DailyReports.Add(relevantDailyReports.ElementAt(i).DailyReport);

                        exporter.ExportToFile(Path.Combine(folder, csvSettings.Filenames[i]));
                        exporter.ExportFlaggedEventsToFile(Path.Combine(folder, csvSettings.EventFilenames[i]));

                        this.AddFile(Path.Combine(folder, csvSettings.Filenames[i]), Resources.FilesLabel_FullExport);
                        this.AddFile(Path.Combine(folder, csvSettings.EventFilenames[i]), Resources.FilesLabel_EventsExport);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationComponentProvider.Status.StatusText = ex.Message;

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }

            ApplicationComponentProvider.Status.ProgressBar = null;
            ApplicationComponentProvider.Status.StatusText = string.Empty;
        }

        #region Button click implementations

        public void BrowseFolder()
        {
            if (this.Files.Count > 0)
            {
                this.OpenFileInExplorer([.. this.Files.Select(f => f.Filename)]);
            }
        }

        public void OpenFileInExplorer(string[] filePaths)
        {
            string arguments = "/select,";

            foreach (string filePath in filePaths)
            {
                arguments += "\"" + filePath + "\",";
            }

            // Remove the trailing comma
            arguments = arguments.TrimEnd(',');

            Process.Start("explorer.exe", arguments);
        }

        #endregion

        #region Event Handlers

        private void FileViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Files));
        }

        private void FileViewModel_FileDeleted(object sender, EventArgs e)
        {
            if (sender is SavedFileViewModel fileViewModel)
            {
                this.Files.Remove(fileViewModel);

                fileViewModel.PropertyChanged -= this.FileViewModel_PropertyChanged;
                fileViewModel.FileDeleted -= this.FileViewModel_FileDeleted;
            }
        }

        private void Exporter_Progress(object sender, ExportProgressEventArgs e)
        {
            if (this.Dispatcher?.CheckAccess() ?? true)
            {
                ApplicationComponentProvider.Status.ProgressBar = new(0, e.ExpectedRows, e.CurrentRowIndex);
                ApplicationComponentProvider.Status.StatusText = string.Format(Resources.RowsWritten, e.CurrentRowIndex.ToString("#,##0"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    ApplicationComponentProvider.Status.ProgressBar = new(0, e.ExpectedRows, e.CurrentRowIndex);
                });
            }
        }

        #endregion

        #endregion
    }
}
