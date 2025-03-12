using cpaplib;

namespace CascadePass.CPAPExporter.Core
{
    public class SignalInfo
    {
        public string Name { get; set; }

        public double FrequencyInHz { get; set; }

        public int SampleCount { get; set; }

        public string UnitOfMeasurement { get; set; }

        public double Sample { get; set; }

        public static List<SignalInfo> ExamineReport(DailyReport dailyReport)
        {
            List<SignalInfo> found = [];

            foreach (var session in dailyReport.Sessions)
            {
                foreach (var signal in session.Signals)
                {
                    if(found.Any(existing => existing.Name == signal.Name))
                    {
                        continue;
                    }

                    SignalInfo info = new()
                    {
                        Name = signal.Name,
                        FrequencyInHz = signal.FrequencyInHz,
                        UnitOfMeasurement = signal.UnitOfMeasurement,
                        SampleCount = signal.Samples.Count,
                    };

                    if (signal.Samples.Count > 0)
                    {
                        info.Sample = signal.Samples[0];
                    }

                    found.Add(info);
                }
            }

            return found;
        }
    }
}
