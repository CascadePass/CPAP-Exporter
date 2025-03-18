using CascadePass.CPAPExporter.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CascadePass.CPAPExporter
{
    public abstract class ExportOptionsViewModel : ViewModel
    {
        private ObservableCollection<ExportFilenamesViewModel> exportFilenames;
        private ExportParameters exportParameters;
        protected ExportSettings settings;

        public ExportOptionsViewModel() : base()
        {
            this.settings = this.CreateSettings();
            this.exportFilenames = [];

            this.exportFilenames.CollectionChanged += this.ExportFilenames_CollectionChanged;
        }

        public ExportParameters ExportParameters
        {
            get => this.exportParameters;
            set => this.SetPropertyValue(ref this.exportParameters, value, nameof(this.ExportParameters));
        }

        public ObservableCollection<ExportFilenamesViewModel> ExportFilenames
        {
            get => this.exportFilenames;
            set => this.SetPropertyValue(ref this.exportFilenames, value, nameof(this.ExportFilenames));
        }

        public ExportSettings Settings
        {
            get => this.settings;
            set => this.SetPropertyValue(ref this.settings, value, nameof(this.Settings));
        }

        public virtual ExportSettings CreateSettings() => new();

        public virtual void CreateFilenames()
        {
            this.exportFilenames.Clear();

            if (this.settings.OutputFileHandling == OutputFileRule.OneFilePerNight)
            {
                foreach (var export in this.ExportParameters.Reports.Where(r => r.IsSelected))
                {
                    var filenames = new ExportFilenamesViewModel
                    {
                        Label = export.DailyReport.ReportDate.ToString("yyyy-MM-dd"),
                        RawFilename = $"{export.DailyReport.ReportDate.ToString("yyyy-MM-dd")}.csv",
                        EventsFilename = this.settings.IncludeEvents ? $"{export.DailyReport.ReportDate.ToString("yyyy-MM-dd")} events.csv" : string.Empty
                    };

                    this.exportFilenames.Add(filenames);
                }
            }
            else
            {
                var filenames = new ExportFilenamesViewModel
                {
                    Label = "Export",
                    RawFilename = $"{this.exportParameters.Reports.First().DailyReport.ReportDate.ToString("yyyy-MM-dd")} - {this.exportParameters.Reports.Last().DailyReport.ReportDate.ToString("yyyy-MM-dd")}.csv",
                    EventsFilename = this.settings.IncludeEvents ? $"{this.exportParameters.Reports.First().DailyReport.ReportDate.ToString("yyyy-MM-dd")} - {this.exportParameters.Reports.Last().DailyReport.ReportDate.ToString("yyyy-MM-dd")} events.csv" : string.Empty
                };

                this.exportFilenames.Add(filenames);
            }
        }

        public virtual void WriteSettings()
        {
            this.settings.Filenames.Clear();

            foreach (var filename in this.exportFilenames)
            {
                this.settings.Filenames.Add(filename.RawFilename);
            }
        }

        private void ExportFilenames_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.WriteSettings();
        }
    }
}
