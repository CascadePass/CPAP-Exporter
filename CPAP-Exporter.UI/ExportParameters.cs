using System.Collections.ObjectModel;
using System.Windows.Data;

namespace CascadePass.CPAPExporter
{
    public class ExportParameters : Observable
    {
        private ObservableCollection<DailyReportViewModel> reports;
        private ObservableCollection<SignalViewModel> signals;
        private List<string> signalNames;
        private string sourcePath, destinationPath;

        public ExportParameters()
        {
            this.Reports = [];
            this.Signals = [];

            BindingOperations.EnableCollectionSynchronization(this.Reports, new object());
            BindingOperations.EnableCollectionSynchronization(this.Signals, new object());
        }

        #region UI Facing

        public ObservableCollection<DailyReportViewModel> Reports
        {
            get => this.reports;
            set => this.SetPropertyValue(ref this.reports, value, nameof(this.Reports));
        }

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

        #endregion

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
    }
}
