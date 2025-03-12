using cpaplib;

namespace CascadePass.CPAPExporter.Core.Tests
{
    [TestClass]
    public class CsvExporterTests
    {
        #region Guard rails (exceptions)

        [TestMethod]
        public void Export_WithNoSignals_ThrowsException()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new DailyReport() { Sessions = [new()] }],
                SignalNamesToExport = []
            };

            Action act = () => exporter.ExportToString();

            var exception = Assert.ThrowsException<InvalidOperationException>(act);

            // Log the exception message, for dev review if necessary.
            Console.WriteLine($"{exporter.DailyReports.Count} Daily Reports:");
            Console.WriteLine($"{exporter.SignalNamesToExport.Count} SignalNamesToExport:");
            Console.WriteLine(exception.Message);
        }

        [TestMethod]
        public void Export_WithNoDailyReports_ThrowsException()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [],
                SignalNamesToExport = ["Flow Rate"]
            };

            Action act = () => exporter.ExportToString();

            var exception = Assert.ThrowsException<InvalidOperationException>(act);

            // Log the exception message, for dev review if necessary.
            Console.WriteLine($"{exporter.DailyReports.Count} Daily Reports:");
            Console.WriteLine($"{exporter.SignalNamesToExport.Count} SignalNamesToExport:");
            Console.WriteLine(exception.Message);
        }

        [TestMethod]
        public void Export_WithNullDailyReports_ThrowsException()
        {
            var exporter = new CsvExporter
            {
                DailyReports = null,
                SignalNamesToExport = ["Signal1"]
            };

            Action act = () => exporter.ExportToString();

            var exception = Assert.ThrowsException<InvalidOperationException>(act);

            // Log the exception message, for dev review if necessary.
            Console.WriteLine(exception.Message);
        }

        [TestMethod]
        public void Export_WithNoSessions_ThrowsException()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new DailyReport() { Sessions = [] }],
                SignalNamesToExport = ["Signal1"]
            };

            Action act = () => exporter.ExportToString();

            var exception = Assert.ThrowsException<InvalidOperationException>(act);

            // Log the exception message, for dev review if necessary.
            Console.WriteLine(exception.Message);
        }

        [TestMethod]
        public void Export_WithNoSignalsInSession_ThrowsException()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new DailyReport() { Sessions = [new Session() { Signals = [] }] }],
                SignalNamesToExport = ["Signal1"]
            };

            Action act = () => exporter.ExportToString();

            var exception = Assert.ThrowsException<InvalidOperationException>(act);

            // Log the exception message, for dev review if necessary.
            Console.WriteLine(exception.Message);
        }

        [TestMethod]
        public void Export_WithNullSignalNamesToExport_ThrowsException()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new DailyReport() { Sessions = [new Session() { Signals = [new Signal() { Name = "Signal1" }] }] }],
                SignalNamesToExport = null
            };

            Action act = () => exporter.ExportToString();

            var exception = Assert.ThrowsException<InvalidOperationException>(act);

            // Log the exception message, for dev review if necessary.
            Console.WriteLine(exception.Message);
        }

        #endregion

        #region Signals export

        [TestMethod]
        public void Export_WithValidData_WritesCsv()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new() {Sessions = [new () { Signals = [ new() { Name = "Flow Rate", FrequencyInHz = 25 } ] } ] } ],
                SignalNamesToExport = ["Flow Rate"]
            };

            exporter.DailyReports[0].Sessions[0].Signals[0].Samples.AddRange([1.1, 2.2]);

            var result = exporter.ExportToString();

            Assert.IsTrue(result.Contains("Flow Rate"));
            Assert.IsTrue(result.Contains("1.1"));
            Assert.IsTrue(result.Contains("2.2"));
        }

        [TestMethod]
        public void Export_WithRowLimit_RespectsLimit()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new() { Sessions = [new Session { Signals = [new() { Name = "Signal1", FrequencyInHz = 1 }] } ] } ],
                SignalNamesToExport = ["Signal1"],
                RowLimit = 3
            };

            exporter.DailyReports[0].Sessions[0].Signals[0].Samples.AddRange([1.1, 2.2, 3.3, 4.4, 5.5]);

            var result = exporter.ExportToString();

            Console.WriteLine(result);

            var lines = result.Split('\n');

            Assert.IsTrue(result.Contains(",Signal1"));
            Assert.IsTrue(lines.Any(l => l.Trim().EndsWith(",1.1")));
            Assert.IsTrue(lines.Any(l => l.Trim().EndsWith(",2.2")));
            Assert.IsTrue(lines.Any(l => l.Trim().EndsWith(",3.3")));
            Assert.IsFalse(lines.Any(l => l.Trim().EndsWith(",4.4")));
            Assert.IsFalse(lines.Any(l => l.Trim().EndsWith(",5.5")));
        }

        [TestMethod]
        public void Export_WithDifferentDelimiter_UsesDelimiter()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new DailyReport { Sessions = [new() { Signals = [new() { Name = "Flow Rate", FrequencyInHz = 25 }] }] }],
                SignalNamesToExport = ["Flow Rate", "Signal1"],
                ExportSettings = new CsvExportSettings { Delimiter = ";" }
            };

            exporter.DailyReports[0].Sessions[0].Signals[0].Samples.AddRange([1.1, 2.2]);

            var result = exporter.ExportToString();

            Console.WriteLine(result);
            Assert.IsTrue(result.Contains(';'), "The entire CSV does not use the Delimiter.");
            Assert.IsTrue(result.Contains(";Signal1"), "Expected signal name proceeded by Delimiter.");
            Assert.IsTrue(result.Contains(";1.1"), "Expected value proceeded by Delimiter.");
            Assert.IsTrue(result.Contains(";2.2"), "Expected value proceeded by Delimiter.");
        }

        [TestMethod]
        public void Export_WithOptionalColumns_IncludesColumns()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new DailyReport { Sessions = [new() { Signals = [new() { Name = "Flow Rate", FrequencyInHz = 25 }] }] }],
                SignalNamesToExport = ["Signal1"],
                ExportSettings = new CsvExportSettings {
                    IncludeColumnHeaders = true,
                    IncludeRowNumber = true,
                    IncludeSessionNumber = true,
                    IncludeTimestamp = true
                },
            };

            exporter.DailyReports[0].Sessions[0].Signals[0].Samples.AddRange([1.0, 2.0]);

            var result = exporter.ExportToString();

            Assert.IsTrue(result.Contains("Row"));
            Assert.IsTrue(result.Contains("Session"));
            Assert.IsTrue(result.Contains("Date"));
        }

        [TestMethod]
        public void Export_WithoutOptionalColumns()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new DailyReport { Sessions = [new() { Signals = [new() { Name = "Flow Rate", FrequencyInHz = 25 }] }] }],
                SignalNamesToExport = ["Signal1"],
                ExportSettings = new CsvExportSettings
                {
                    IncludeColumnHeaders = true,
                    IncludeRowNumber = false,
                    IncludeSessionNumber = false,
                    IncludeTimestamp = false
                }
            };

            exporter.DailyReports[0].Sessions[0].Signals[0].Samples.AddRange([1.0, 2.0]);

            var result = exporter.ExportToString();

            Assert.IsFalse(result.Contains("Row"));
            Assert.IsFalse(result.Contains("Session"));
            Assert.IsFalse(result.Contains("Date"));
        }

        [TestMethod]
        public void Export_WithEmptySignalNames_HandlesEmptyStrings()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new() { Sessions = [new() { Signals = [new() { Name = "Flow Rate", FrequencyInHz = 25 }] }] }],
                SignalNamesToExport = ["Flow Rate", "", " "]
            };

            exporter.DailyReports[0].Sessions[0].Signals[0].Samples.AddRange([1.1, 2.2]);

            var result = exporter.ExportToString();

            Console.WriteLine(result);
            Assert.IsTrue(result.Contains("Flow Rate"), "Desired (real) signal name not found");
            Assert.IsTrue(result.Contains("1.1"), "First data row not found.");
            Assert.IsTrue(result.Contains("2.2"), "Second data row not found.");
        }

        [TestMethod]
        public void Export_WithSpecialCharactersInData_HandlesSpecialCharacters()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new() { Sessions = [new() {
                        Signals =
                        [
                            new Signal { Name = "Signal1", FrequencyInHz = 1 },
                            new Signal { Name = "Signal,With,Comma", FrequencyInHz = 1 }
                        ]}]}],
                SignalNamesToExport = new List<string> { "Signal1", "Signal,With,Comma" }
            };

            exporter.DailyReports[0].Sessions[0].Signals[0].Samples.AddRange([1.1, 2.2]);
            exporter.DailyReports[0].Sessions[0].Signals[1].Samples.AddRange([3.3, 4.4]);

            var result = exporter.ExportToString();

            Console.WriteLine(result);
            Assert.IsTrue(result.Contains("Signal1"));
            Assert.IsTrue(result.Contains("\"Signal,With,Comma\""));
            Assert.IsTrue(result.Contains("1.1"));
            Assert.IsTrue(result.Contains("2.2"));
            Assert.IsTrue(result.Contains("3.3"));
            Assert.IsTrue(result.Contains("4.4"));
        }

        [TestMethod]
        public void Export_IncludesColumnHeadersWhenDesired()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new() { Sessions = [new() { Signals = [new() { Name = "Flow Rate", FrequencyInHz = 25 }, new() { Name = "Pressure", FrequencyInHz = .5 }] }] }],
                SignalNamesToExport = ["Flow Rate", "Pressure"],
                ExportSettings = new CsvExportSettings { IncludeColumnHeaders = true }
            };

            exporter.DailyReports[0].Sessions[0].Signals[0].Samples.AddRange([1.1, 2.2]);

            var result = exporter.ExportToString();

            string[] lines = result.Split('\n');
            Assert.IsTrue(lines[0].Contains(",Flow Rate"));
            Assert.IsTrue(lines[0].Contains(",Pressure"));
        }

        [TestMethod]
        public void Export_ExcludesColumnHeadersWhenNotDesired()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [new() { Sessions = [new() { Signals = [new() { Name = "Flow Rate", FrequencyInHz = 25 }, new() { Name = "Pressure", FrequencyInHz = .5 }] }] }],
                SignalNamesToExport = ["Flow Rate", "Pressure"],
                ExportSettings = new CsvExportSettings { IncludeColumnHeaders = false }
            };

            exporter.DailyReports[0].Sessions[0].Signals[0].Samples.AddRange([1.1, 2.2]);

            var result = exporter.ExportToString();

            string[] lines = result.Split('\n');
            Assert.IsFalse(lines[0].Contains(",Flow Rate"));
            Assert.IsFalse(lines[0].Contains(",Pressure"));
        }

        #endregion

        #region Events export

        #region Data within CSV

        [TestMethod]
        public void ExportFlaggedEventsToFile_WithValidData_WritesCsv()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [ new() {
                        Events =
                        [
                            new() { Type = EventType.ObstructiveApnea, StartTime = DateTime.Now, Duration = TimeSpan.FromSeconds(30), SourceType = SourceType.CPAP },
                            new() { Type = EventType.Hypopnea, StartTime = DateTime.Now.AddMinutes(1), Duration = TimeSpan.FromSeconds(20), SourceType = SourceType.CPAP }
                        ]
                    }
                ]
            };

            string filePath = Path.GetTempFileName();
            exporter.ExportFlaggedEventsToFile(filePath);

            var result = File.ReadAllText(filePath);

            File.Delete(filePath);

            Assert.IsTrue(result.Contains("Event Type,StartTime,Duration"));
            Assert.IsTrue(result.Contains(EventType.ObstructiveApnea.ToString()));
            Assert.IsTrue(result.Contains(EventType.Hypopnea.ToString()));
        }

        [TestMethod]
        public void ExportFlaggedEventsToString_WithValidData_ReturnsCsvString()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [ new() {
                        Events =
                        [
                            new() { Type = EventType.ObstructiveApnea, StartTime = DateTime.Now, Duration = TimeSpan.FromSeconds(30), SourceType = SourceType.CPAP },
                            new() { Type = EventType.Hypopnea, StartTime = DateTime.Now.AddMinutes(1), Duration = TimeSpan.FromSeconds(20), SourceType = SourceType.CPAP }
                        ]
                    }
                ]
            };

            var result = exporter.ExportFlaggedEventsToString();

            Assert.IsTrue(result.Contains("Event Type,StartTime,Duration"));
            Assert.IsTrue(result.Contains(EventType.ObstructiveApnea.ToString()));
            Assert.IsTrue(result.Contains(EventType.Hypopnea.ToString()));
        }

        [TestMethod]
        public void ExportFlaggedEvents_WithNoEvents_WritesEmptyCsv()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [ new() {
                        Events = []
                    }
                ]
            };

            var result = exporter.ExportFlaggedEventsToString();

            Console.WriteLine(result);
            Assert.IsTrue(result.Contains("Event Type,StartTime,Duration"));

            foreach(var eventType in Enum.GetValues<EventType>())
            {
                Assert.IsFalse(result.Contains(eventType.ToString()), $"{eventType} found in CSV output which should be blank");
            }
        }

        #endregion

        #region Headers

        [TestMethod]
        public void ExportFlaggedEvents_HeadersPresentWhenRequested()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [ new() {
                        Events =
                        [
                            new() { Type = EventType.ObstructiveApnea, StartTime = DateTime.Now, Duration = TimeSpan.FromSeconds(30), SourceType = SourceType.CPAP },
                            new() { Type = EventType.Hypopnea, StartTime = DateTime.Now.AddMinutes(1), Duration = TimeSpan.FromSeconds(20), SourceType = SourceType.CPAP }
                        ]
                    }
                ],
                ExportSettings = new CsvExportSettings { IncludeColumnHeaders = true }
            };

            var result = exporter.ExportFlaggedEventsToString();

            Console.WriteLine(result);
            Assert.IsTrue(result.Contains("Event Type,StartTime,Duration"));
        }

        [TestMethod]
        public void ExportFlaggedEvents_HeadersNotPresentWhenNotRequested()
        {
            var exporter = new CsvExporter
            {
                DailyReports = [ new() {
                        Events =
                        [
                            new() { Type = EventType.ObstructiveApnea, StartTime = DateTime.Now, Duration = TimeSpan.FromSeconds(30), SourceType = SourceType.CPAP },
                            new() { Type = EventType.Hypopnea, StartTime = DateTime.Now.AddMinutes(1), Duration = TimeSpan.FromSeconds(20), SourceType = SourceType.CPAP }
                        ]
                    }
                ],
                ExportSettings = new CsvExportSettings { IncludeColumnHeaders = false }
            };

            var result = exporter.ExportFlaggedEventsToString();

            Console.WriteLine(result);
            Assert.IsFalse(result.Contains("Event Type,StartTime,Duration"));
        }
        #endregion

        #endregion
    }
}
