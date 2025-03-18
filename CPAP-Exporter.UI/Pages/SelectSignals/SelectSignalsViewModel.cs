using CascadePass.CPAPExporter.Core;
using cpaplib;
using System.Collections.ObjectModel;

namespace CascadePass.CPAPExporter
{
    public class SelectSignalsViewModel : PageViewModel
    {
        private ExportDetails exportDetails;
        private List<SignalInfo> signalDescriptions;

        #region Constructors

        public SelectSignalsViewModel() : base(Resources.PageTitle_SelectSignals, Resources.PageDesc_SelectSignals)
        {
            this.MaxSampleLength = 400;
        }

        public SelectSignalsViewModel(ExportParameters exportParameters) : this()
        {
            this.ExportParameters = exportParameters;
        }

        #endregion

        #region Properties

        public int MaxSampleLength { get; set; }

        public ObservableCollection<DailyReportViewModel> Reports => this.ExportParameters.Reports;

        public ObservableCollection<SignalViewModel> Signals => this.ExportParameters.Signals;

        public ExportDetails ExportDetails
        {
            get => this.exportDetails;
            set => this.SetPropertyValue(ref this.exportDetails, value, nameof(this.ExportDetails));
        }

        public List<SignalInfo> SignalDescriptions
        {
            get => this.signalDescriptions;
            set => this.SetPropertyValue(ref this.signalDescriptions, value, nameof(this.SignalDescriptions));
        }

        public string SampleCSV => this.GenerateSampleCSV();

        #endregion

        public string GenerateSampleCSV()
        {
            List<DailyReport> reports = [];
            List<string> signalNames = [];

            foreach (var report in this.Reports.Where(r => r.IsSelected))
            {
                reports.Add(report.DailyReport);
            }

            foreach (var signal in this.Signals.Where(s => s.IsSelected))
            {
                signalNames.Add(signal.SignalInfo.Name);
            }

            if (signalNames.Count == 0 && reports.Count > 0)
            {
                foreach (Session session in reports[0].Sessions)
                {
                    foreach (Signal signal in session.Signals)
                    {
                        if (!signalNames.Contains(signal.Name))
                        {
                            signalNames.Add(signal.Name);
                        }
                    }
                }
            }

            if (reports.Count > 0 && signalNames.Count > 0)
            {
                CsvExporter csvExporter = new(reports, signalNames) { RowLimit = 10 };

                return csvExporter.ExportToString();
            }

            return string.Empty;
        }

        public override bool Validate()
        {
            return this.Signals.Any(signal => signal.IsSelected);
        }

        private void ConsumeExportParameters()
        {
            this.SignalDescriptions = SignalInfo.ExamineReport(this.ExportParameters.Reports.Select(r => r.DailyReport).Last());
            this.ExportDetails = new ExportDetails([.. this.Reports.Where(r => r.IsSelected).Select(r => r.DailyReport)]);

            this.Signals.Clear();
            foreach (var signalDescription in this.SignalDescriptions)
            {
                this.Signals.Add(new SignalViewModel(signalDescription));
            }

            if (this.ExportParameters.Signals.Count == 0)
            {
                foreach (var signal in this.Signals)
                {
                    if(signal.SignalInfo.FrequencyInHz > 1)
                    {
                        this.ExportDetails.HighResolutionSampleNames.Add(signal.SignalInfo.Name);
                    }
                    else
                    {
                        this.ExportDetails.NormalResolutionSampleNames.Add(signal.SignalInfo.Name);
                    }
                }
            }

            ApplicationComponentProvider.Status.StatusText = string.Format(Resources.SignalsAvailable, this.ExportParameters.Signals.Count);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(this.ExportParameters))
            {
                this.ConsumeExportParameters();
            }
        }
    }
}
