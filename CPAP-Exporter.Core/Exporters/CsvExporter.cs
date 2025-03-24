using CascadePass.CPAPExporter.Core.Aggregates;
using cpaplib;
using System.IO;
using System.Text;

namespace CascadePass.CPAPExporter.Core
{
    /// <summary>
    /// Provides functionality to export CPAP data to CSV format.
    /// </summary>
    /// <remarks>
    /// The <see cref="CsvExporter"/> class is responsible for exporting PAP data to a CSV file. It supports
    /// various configuration options such as including headers, row numbers, session numbers, and report dates.
    /// The class can be initialized with default settings or with specific daily reports and signal names to export.
    /// </remarks>
    public class CsvExporter : Exporter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvExporter"/> class with default settings.
        /// </summary>
        /// <remarks>
        /// This constructor sets the default values for the instance, including enabling the inclusion of headers,
        /// row numbers, session numbers, and report dates in the CSV output.
        /// </remarks>
        public CsvExporter()
        {
            this.ExportSettings = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvExporter"/> class with the specified daily reports and signal names to export.
        /// </summary>
        /// <param name="dailyReports">The list of daily reports to export.</param>
        /// <param name="signalNamesToExport">The list of signal names to include in the export.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="dailyReports"/> or <paramref name="signalNamesToExport"/> is null.
        /// </exception>
        /// <remarks>
        /// This constructor initializes the instance with the provided daily reports and signal names to export,
        /// and sets the default values for the instance, including enabling the inclusion of headers,
        /// row numbers, session numbers, and report dates in the CSV output.
        /// </remarks>
        public CsvExporter(List<DailyReport> dailyReports, List<string> signalNamesToExport) : this()
        {
            this.DailyReports = dailyReports ?? throw new ArgumentNullException(nameof(dailyReports));
            this.SignalNamesToExport = signalNamesToExport ?? throw new ArgumentNullException(nameof(signalNamesToExport));
        }

        #endregion

        #region Properties

        public override string Name => Resources.CSV;

        public override string Description => Resources.CSV_Description;

        public new CsvExportSettings ExportSettings { get; set; }

        #region Control Output

        /// <summary>
        /// Gets or sets the maximum number of rows to export.
        /// </summary>
        /// <value>
        /// The maximum number of rows to export. If set to 0 or a negative value, there is no limit.
        /// </value>
        public int RowLimit { get; set; }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Exports the PAP data to the provided stream in CSV format.
        /// </summary>
        /// <param name="stream">The stream to write the CSV data to. Must be a valid, open stream.</param>
        /// <exception cref="ArgumentNullException">Thrown if the stream is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the DailyReports property is null, or if any DailyReport does not have sessions,
        /// or if any session does not have signals.
        /// </exception>
        /// <remarks>
        /// This method writes the PAP data to the provided stream in CSV format. It includes optional columns
        /// such as Row, Session, and Date if their respective properties are set to true. The data is written
        /// based on the configuration of the CsvExporter instance, including the specified signal names to export.
        /// The stream is flushed periodically to ensure data integrity in case of unexpected failures.
        /// </remarks>
        public override void Export(Stream stream)
        {
            #region Sanity Checks

            ArgumentNullException.ThrowIfNull(stream);

            if (this.DailyReports == null)
            {
                throw new InvalidOperationException(Resources.Validation_DailyReports_Null);
            }

            if (this.DailyReports.Count == 0)
            {
                throw new InvalidOperationException(Resources.Validation_DailyReports_Empty);
            }

            if (this.DailyReports.Any(r => r.Sessions?.Count == 0))
            {
                throw new InvalidOperationException(Resources.Validation_DailyReports_NoSessions);
            }

            if (this.DailyReports.Any(r => r.Sessions.Any(s => s.Signals?.Count == 0)))
            {
                throw new InvalidOperationException(Resources.Validation_DailyReports_NoSignals);
            }

            if (this.SignalNamesToExport == null)
            {
                throw new InvalidOperationException(Resources.Validation_SignalNames_Null);
            }

            if (this.SignalNamesToExport.Count == 0)
            {
                throw new InvalidOperationException(Resources.Validation_SignalNames_Empty);
            }

            #endregion

            var details = base.ExamineSignals();

            var exportSignals = new HashSet<string>(this.SignalNamesToExport);
            using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);

            if (this.ExportSettings.IncludeColumnHeaders)
            {
                this.WriteColumnHeaders(writer);
            }

            int rowCount = 0;
            foreach (var report in this.DailyReports)
            {
                for (int sessionIndex = 0; sessionIndex < report.Sessions.Count; sessionIndex++)
                {
                    var session = report.Sessions[sessionIndex];

                    for (int y_sampleIndex = 0; y_sampleIndex < details.CountExpectedSamples(session); y_sampleIndex++)
                    {
                        if (rowCount >= this.RowLimit && this.RowLimit > 0)
                        {
                            writer.Flush();
                            return;
                        }

                        ExportRow data = new(rowCount, sessionIndex, session.StartTime.AddSeconds(2 * y_sampleIndex));

                        for (int x_signalIndex = 0; x_signalIndex < session.Signals.Count; x_signalIndex++)
                        {
                            var signal = session.Signals[x_signalIndex];

                            if (exportSignals.Contains(signal.Name))
                            {
                                string value = string.Empty;

                                if (details.HighResolutionSampleNames.Contains(signal.Name))
                                {
                                    // Should be a streaming aggregate for every high resolution column?
                                    StreamingAverage streamingAverage = new();

                                    var factor = details.DownsamplingFactors[signal.Name];
                                    for (int i = (int)(y_sampleIndex * factor); i < factor; i++)
                                    {
                                        streamingAverage.Sample(signal.Samples[i]);
                                    }

                                    value = streamingAverage.Value.ToString();
                                }
                                else
                                {
                                    if (y_sampleIndex < signal.Samples.Count)
                                    {
                                        value = $"{signal.Samples[y_sampleIndex]}";
                                    }
                                }

                                data.Values[signal.Name] = value;

                                //this.WriteValue(writer, value, x_signalIndex < session.Signals.Count - 1);
                            }
                        }

                        this.Write(writer, data);
                        rowCount += 1;

                        #region Politeness

                        // If the SD drive is pulled out while the export is in progress,
                        // the user will at least be able to preview whatever data has been
                        // written so far.  So I flush the buffer periodically.

                        if (rowCount % 100 == 0)
                        {
                            writer.Flush();
                        }

                        #endregion

                        #region Report Progress

                        if (rowCount % 100 == 0)
                        {
                            this.OnProgress(rowCount, details.ExpectedSampleCount);
                        }

                        #endregion
                    }
                }
            }

            writer.Flush();
        }

        /// <summary>
        /// Writes the column headers to the provided StreamWriter.
        /// </summary>
        /// <param name="writer">The StreamWriter to write to. Must be a valid, open StreamWriter.</param>
        /// <exception cref="ArgumentNullException">Thrown if the stream is null.</exception>
        /// <remarks>
        /// This method writes the column headers based on the configuration of the CsvExporter instance.
        /// It includes optional columns such as Row, Session, and Date if their respective properties are set to true.
        /// It also writes the headers for the signals specified in the SignalNamesToExport list, excluding any signal named "Events".
        /// </remarks>
        private void WriteColumnHeaders(StreamWriter writer)
        {
            #region Sanity Checks

            ArgumentNullException.ThrowIfNull(writer);

            #endregion

            #region Optional non-signal columns

            if (this.ExportSettings.IncludeRowNumber)
            {
                this.WriteValue(writer, Resources.ColumnHeader_Row, true);
            }

            if (this.ExportSettings.IncludeSessionNumber)
            {
                this.WriteValue(writer, Resources.ColumnHeader_Session, true);
            }

            if (this.ExportSettings.IncludeTimestamp)
            {
                this.WriteValue(writer, Resources.ColumnHeader_Date, true);
            }

            #endregion

            for (int i = 0; i < this.SignalNamesToExport.Count; i++)
            {
                string signalName = this.SignalNamesToExport[i];
                if (string.Equals(Resources.Events, signalName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                this.WriteValue(writer, signalName, i < this.SignalNamesToExport.Count - 1);
            }

            writer.WriteLine();
        }

        private void Write(StreamWriter writer, ExportRow data)
        {
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(data);

            if (this.ExportSettings.IncludeRowNumber)
            {
                this.WriteValue(writer, $"{data.RowNumber}", true);
            }

            if (this.ExportSettings.IncludeSessionNumber)
            {
                this.WriteValue(writer, $"{data.SessionNumber}", true);
            }

            if (this.ExportSettings.IncludeTimestamp)
            {
                this.WriteValue(writer, $"{data.Timestamp}", true);
            }

            foreach (var signalName in this.SignalNamesToExport)
            {
                if (string.Equals(Resources.Events, signalName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (data.Values.TryGetValue(signalName, out string value))
                {
                    this.WriteValue(writer, value, signalName != this.SignalNamesToExport.Last());
                }
                else
                {
                    this.WriteValue(writer, string.Empty, signalName != this.SignalNamesToExport.Last());
                }
            }

            writer.WriteLine();
        }

        /// <summary>
        /// Writes a value to the provided StreamWriter.
        /// </summary>
        /// <param name="writer">The StreamWriter to write to. Must be a valid, open StreamWriter.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="appendComma">Whether to append a comma after the value.</param>
        /// <remarks>
        /// This method assumes that the StreamWriter has already been validated by the caller.
        /// </remarks>
        private void WriteValue(StreamWriter writer, string value, bool appendComma)
        {
            value ??= string.Empty;

            if (value.Contains(this.ExportSettings.Delimiter))
            {
                writer.Write($"\"{value}\"");
            }
            else
            {
                writer.Write($"{value}");
            }

            if (appendComma)
            {
                writer.Write(this.ExportSettings.Delimiter);
            }
        }

        #region Events

        /// <summary>
        /// Exports the flagged events to a specified file in CSV format.
        /// </summary>
        /// <param name="filePath">The path of the file to write the CSV data to.</param>
        /// <exception cref="ArgumentNullException">Thrown if the filePath is null or empty.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the DailyReports property is null, or if any DailyReport does not have sessions,
        /// or if any session does not have signals.
        /// </exception>
        /// <remarks>
        /// This method writes the flagged events to the specified file in CSV format. It includes columns
        /// such as Event Type, StartTime, and Duration. The data is written based on the flagged events
        /// present in the DailyReports.
        /// </remarks>
        public void ExportFlaggedEventsToFile(string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            this.ExportFlaggedEvents(fileStream);
        }

        /// <summary>
        /// Exports the flagged events to a string in CSV format.
        /// </summary>
        /// <returns>A string containing the flagged events in CSV format.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the DailyReports property is null, or if any DailyReport does not have sessions,
        /// or if any session does not have signals.
        /// </exception>
        /// <remarks>
        /// This method returns the flagged events as a string in CSV format. It includes columns
        /// such as Event Type, StartTime, and Duration. The data is written based on the flagged events
        /// present in the DailyReports.
        /// </remarks>
        public string ExportFlaggedEventsToString()
        {
            using var memoryStream = new MemoryStream();
            this.ExportFlaggedEvents(memoryStream);
            memoryStream.Position = 0;

            using var reader = new StreamReader(memoryStream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Exports the flagged events to the provided stream in CSV format.
        /// </summary>
        /// <param name="stream">The stream to write the CSV data to. Must be a valid, open stream.</param>
        /// <exception cref="ArgumentNullException">Thrown if the stream is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the DailyReports property is null, or if any DailyReport does not have sessions,
        /// or if any session does not have signals.
        /// </exception>
        /// <remarks>
        /// This method writes the flagged events to the provided stream in CSV format. It includes columns
        /// such as Event Type, StartTime, and Duration. The data is written based on the flagged events
        /// present in the DailyReports.
        /// </remarks>
        private void ExportFlaggedEvents(Stream stream)
        {
            using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);

            if (this.ExportSettings.IncludeColumnHeaders)
            {
                writer.WriteLine($"{Resources.ColumnHeader_EventType},{Resources.ColumnHeader_StartTime},{Resources.ColumnHeader_Duration}");
            }

            foreach (var report in this.DailyReports)
            {
                foreach (var flaggedEvent in report.Events.Where(e => e.SourceType == SourceType.CPAP))
                {
                    this.WriteValue(writer, $"{flaggedEvent.Type}", true);
                    this.WriteValue(writer, $"{flaggedEvent.StartTime}", true);
                    this.WriteValue(writer, $"{flaggedEvent.Duration}", false);
                    writer.WriteLine();
                }
            }
        }

        #endregion

        #endregion
    }
}
