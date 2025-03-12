using CascadePass.CPAPExporter.Core;
using cpaplib;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace CascadePass.CPAPExporter.Core
{
    /// <summary>
    /// Represents the base class for all exporters.
    /// </summary>
    public abstract class Exporter
    {
        #region Properties

        /// <summary>
        /// Gets the name of the exporter.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description of the exporter.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets or sets the list of daily reports to be exported.
        /// </summary>
        public List<DailyReport> DailyReports { get; set; }

        /// <summary>
        /// Gets or sets the list of signal names to be exported.
        /// </summary>
        public List<string> SignalNamesToExport { get; set; }

        public ExportSettings ExportSettings { get; set; }

        #endregion

        #region Methods

        public static List<Exporter> GetExporters()
        {
            var exporters = new List<Exporter>();
            var exporterTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Exporter)) && !t.IsAbstract);

            foreach (var type in exporterTypes)
            {
                var exporter = (Exporter)Activator.CreateInstance(type);
                exporters.Add(exporter);
            }

            return exporters;
        }

        public static List<Exporter> GetExporters(string folderPath)
        {
            throw new NotImplementedException();
        }

        #region Main data

        public abstract void Export(Stream stream);

        public void ExportToFile(string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            this.Export(fileStream);
        }

        public string ExportToString()
        {
            using var memoryStream = new MemoryStream();
            this.Export(memoryStream);
            memoryStream.Position = 0;

            using var reader = new StreamReader(memoryStream);
            return reader.ReadToEnd();
        }

        #endregion

        internal ExportDetails ExamineSignals()
        {
            ExportDetails details = new(this.DailyReports);

            // Bucket signals by cardinality
            //foreach (var report in this.DailyReports)
            //{
            //    foreach (Session session in report.Sessions)
            //    {
            //        foreach (Signal signal in session.Signals)
            //        {
            //            if (!this.SignalNamesToExport.Contains(signal.Name))
            //            {
            //                continue;
            //            }

            //            if (details.LowResolutionSampleNames.Contains(signal.Name) || details.HighResolutionSampleNames.Contains(signal.Name))
            //            {
            //                continue;
            //            }

            //            if (signal.FrequencyInHz <= 1)
            //            {
            //                details.LowResolutionSampleNames.Add(signal.Name);
            //            }
            //            else
            //            {
            //                details.HighResolutionSampleNames.Add(signal.Name);
            //            }
            //        }
            //    }
            //}

            // Count all samples for each signal
            //foreach (var report in this.DailyReports)
            //{
            //    foreach (Session session in report.Sessions)
            //    {
            //        foreach (Signal signal in session.Signals)
            //        {
            //            if (!details.LowResolutionSampleNames.Contains(signal.Name))
            //            {
            //                continue;
            //            }

            //            if (!details.SampleCountsBySignal.ContainsKey(signal.Name))
            //            {
            //                details.SampleCountsBySignal[signal.Name] = signal.Samples.Count;
            //            }
            //            else
            //            {
            //                if (details.SampleCountsBySignal[signal.Name] < signal.Samples.Count)
            //                {
            //                    details.SampleCountsBySignal[signal.Name] = signal.Samples.Count;
            //                }
            //            }
            //        }
            //    }
            //}

            // Find the largest number of normal rate samples
            //foreach (var item in details.SampleCountsBySignal)
            //{
            //    if (item.Value > details.ExpectedSampleCount)
            //    {
            //        details.ExpectedSampleCount = item.Value;
            //    }
            //}

            // Calculate downsampling factors
            //if (details.HasMixedFrequencies)
            //{
            //    var lowResFrequency = this.DailyReports
            //        .SelectMany(report => report.Sessions)
            //        .SelectMany(session => session.Signals)
            //        .Where(signal => details.LowResolutionSampleNames.Contains(signal.Name))
            //        .Select(signal => signal.FrequencyInHz)
            //        .FirstOrDefault();

            //    foreach (var highResSignal in details.HighResolutionSampleNames)
            //    {
            //        var highResFrequency = this.DailyReports
            //            .SelectMany(report => report.Sessions)
            //            .SelectMany(session => session.Signals)
            //            .Where(signal => signal.Name == highResSignal)
            //            .Select(signal => signal.FrequencyInHz)
            //            .FirstOrDefault();

            //        if (lowResFrequency > 0 && highResFrequency > 0)
            //        {
            //            details.DownsamplingFactors[highResSignal] = highResFrequency / lowResFrequency;
            //        }
            //    }
            //}

            return details;
        }

        #endregion
    }
}
