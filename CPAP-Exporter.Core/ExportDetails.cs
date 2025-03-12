using cpaplib;

namespace CascadePass.CPAPExporter.Core
{
    /// <summary>
    /// ExportDetails is a utility class that provides information
    /// about the signals that are being exported.
    /// </summary>
    public class ExportDetails
    {
        #region Constructor

        private ExportDetails()
        {
        }

        public ExportDetails(List<DailyReport> dailyReports)
        {
            ExportDetails.ValidateReports(dailyReports);

            this.SplitSignalsBySampleFrequency(dailyReports);
            this.CountSamplesPerSignal(dailyReports);
            this.FindExpectedSampleCount(dailyReports);
            this.CalculateDownSampleFactors(dailyReports);
        }

        #endregion

        #region Properties

        public HashSet<string> NormalResolutionSampleNames { get; internal set; }

        public HashSet<string> HighResolutionSampleNames { get; internal set; }

        public bool HasMixedFrequencies =>
            this.NormalResolutionSampleNames.Count > 0 &&
            this.HighResolutionSampleNames.Count > 0
        ;

        public int ExpectedSampleCount { get; internal set; }

        public Dictionary<string, int> SampleCountsBySignal { get; internal set; }

        public Dictionary<string, double> DownsamplingFactors { get; internal set; }

        #endregion

        #region Methods

        private static void ValidateReports(List<DailyReport> dailyReports)
        {
            ArgumentNullException.ThrowIfNull(dailyReports);

            if (dailyReports.Count == 0)
            {
                throw new ArgumentException("The daily reports list cannot be empty.", nameof(dailyReports));
            }

            if(dailyReports.Any(report => report.Sessions.Count == 0))
            {
                throw new ArgumentException("All daily reports must contain at least one session.", nameof(dailyReports));
            }
        }

        public void SplitSignalsBySampleFrequency(List<DailyReport> dailyReports)
        {
            var signals = dailyReports
                .SelectMany(report => report.Sessions)
                .SelectMany(session => session.Signals)
                .Where(signal => signal.FrequencyInHz > 0)
                .ToList();

            if(signals.Count == 0)
            {
                return;
            }

            var minFrequency = signals.Min(signal => signal.FrequencyInHz);

            this.NormalResolutionSampleNames =
                [.. signals
                .Where(signal => signal.FrequencyInHz == minFrequency)
                .Select(signal => signal.Name)
                ];

            this.HighResolutionSampleNames =
                [.. signals
                .Where(signal => signal.FrequencyInHz != minFrequency)
                .Select(signal => signal.Name)
                ];
        }

        public void CountSamplesPerSignal(List<DailyReport> dailyReports)
        {
            this.SampleCountsBySignal = dailyReports
                .SelectMany(report => report.Sessions)
                .SelectMany(session => session.Signals)
                .GroupBy(signal => signal.Name)
                .ToDictionary(
                    group => group.Key,
                    group => group.Max(signal => signal.Samples.Count)
                );
        }

        public void FindExpectedSampleCount(List<DailyReport> dailyReports)
        {
            if(this.HighResolutionSampleNames is null || this.HighResolutionSampleNames.Count == 0)
            {
                this.ExpectedSampleCount = this.SampleCountsBySignal.Values.Max();
                return;
            }

            this.ExpectedSampleCount = this.SampleCountsBySignal.Where(s => !this.HighResolutionSampleNames.Contains(s.Key)).Select(i => i.Value).Max();
        }

        public void CalculateDownSampleFactors(List<DailyReport> dailyReports)
        {
            this.DownsamplingFactors = [];

            var lowResFrequency = dailyReports
                .SelectMany(report => report.Sessions)
                .SelectMany(session => session.Signals)
                .Where(signal => this.NormalResolutionSampleNames.Contains(signal.Name))
                .Select(signal => signal.FrequencyInHz)
                .FirstOrDefault();

            foreach (var highResSignal in this.HighResolutionSampleNames)
            {
                var highResFrequency = dailyReports
                    .SelectMany(report => report.Sessions)
                    .SelectMany(session => session.Signals)
                    .Where(signal => signal.Name == highResSignal)
                    .Select(signal => signal.FrequencyInHz)
                    .FirstOrDefault();

                if (lowResFrequency > 0 && highResFrequency > 0)
                {
                    this.DownsamplingFactors[highResSignal] = highResFrequency / lowResFrequency;
                }
            }
        }

        public int CountExpectedSamples(Session session)
        {
            return session.Signals
                .Where(signal => !this.HighResolutionSampleNames.Contains(signal.Name))
                .Max(signal => signal.Samples.Count);
        }

        #endregion
    }
}
