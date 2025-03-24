using cpaplib;
using System.Data;
using System.IO;
using System.Reflection;

namespace CascadePass.CPAPExporter.Core
{
    /// <summary>
    /// Represents the base class for all exporters.
    /// </summary>
    public abstract class Exporter
    {
        public event EventHandler<ExportProgressEventArgs> Progress;

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
            return new(this.DailyReports);
        }

        internal void OnProgress(int current, int expected)
        {
            this.OnProgress(this, new() { CurrentRowIndex = current, ExpectedRows = expected, });
        }

        internal void OnProgress(object sender, ExportProgressEventArgs eventArgs)
        {
            this.Progress?.Invoke(sender, eventArgs);
        }

        #endregion
    }
}
