using cpaplib;

namespace CascadePass.CPAPExporter
{
    public class DailyReportViewModel : ViewModel
    {
        private DailyReport dailyReport;
        private bool isSelected;
        private int? sampleCount;

        #region Constructors

        public DailyReportViewModel()
        {
            this.isSelected = true;
        }

        public DailyReportViewModel(DailyReport report, bool initiallySelected)
        {
            this.dailyReport = report;
            this.isSelected = initiallySelected;
        }

        #endregion

        public bool IsSelected
        {
            get => this.isSelected;
            set => this.SetPropertyValue(ref this.isSelected, value, nameof(this.IsSelected));
        }

        public DailyReport DailyReport {
            get => this.dailyReport;
            set => this.SetPropertyValue(ref this.dailyReport, value, nameof(this.DailyReport));
        }

        public int SampleCount
        {
            get
            {
                if (this.sampleCount == null)
                {
                    int count = 0;

                    foreach (var session in this.DailyReport.Sessions)
                    {
                        Signal testSignal = session.Signals.FirstOrDefault(s => s.FrequencyInHz <= 1);

                        if (testSignal != null)
                        {
                            count += testSignal.Samples.Count;
                        }
                    }

                    this.sampleCount = count;
                }

                return this.sampleCount.Value;
            }
        }

        public string TherapyMode => this.DailyReport?.Settings["Mode"]?.ToString();

        public string PressureDescription
        {
            get
            {
                var mode = this.DailyReport.Settings["Mode"];

                switch (mode.ToString())
                {
                    case "AsvVariableEpap":
                        object
                            minPS = this.DailyReport.Settings["PS Minimum"],
                            maxPS = this.DailyReport.Settings["PS Maximum"],
                            minEPAP = this.DailyReport.Settings["EPAP Min"],
                            maxEPAP = this.DailyReport.Settings["EPAP Max"];

                        return $"{minPS} - {maxPS} Over {minEPAP} - {maxEPAP}";

                    case "BilevelAutoFixedPS":
                        double
                            autoIPAP = (double)this.DailyReport.Settings["IPAP"],
                            autoEPAP = (double)this.DailyReport.Settings["EPAP"];

                        return $"{autoIPAP - autoEPAP} PS Over {autoEPAP}";

                    case "BilevelFixed":
                        double
                            fixedIPAP = (double)this.DailyReport.Settings["IPAP"],
                            fixedEPAP = (double)this.DailyReport.Settings["EPAP"];

                        return $"{fixedIPAP-fixedEPAP} PS Over {fixedEPAP}";

                    default:
                        System.Diagnostics.Debug.WriteLine(string.Empty);
                        System.Diagnostics.Debug.WriteLine(string.Empty);
                        System.Diagnostics.Debug.WriteLine($"{mode}");

                        foreach (var item in this.DailyReport.Settings)
                        {
                            System.Diagnostics.Debug.WriteLine($"{item.Key} = {item.Value}");
                        }

                        System.Diagnostics.Debug.WriteLine(string.Empty);

                        return null;
                }
            }
        }
    }
}
