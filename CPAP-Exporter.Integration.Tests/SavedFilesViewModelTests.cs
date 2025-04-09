using CascadePass.CPAPExporter.Core;

namespace CascadePass.CPAPExporter.Integration.Tests
{
    //[TestClass]
    [DoNotParallelize]
    public class SavedFilesViewModelTests
    {
        private List<string> tempFolder;

        public SavedFilesViewModelTests()
        {
            this.tempFolder = [];
        }

        [TestMethod]
        public void PerformExport_SingleReport_OneFilePerNight_IncludEvents_DefaultFilenames()
        {
            var exportParams = new ExportParameters();
            var source = TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH);

            var selectNightsViewModel = new SelectNightsViewModel(exportParams);

            string folder = exportParams.DestinationPath = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString());
            this.tempFolder.Add(folder);

            selectNightsViewModel.LoadFromFolder(source, true);

            var selectSignalsViewModel = new SelectSignalsViewModel(exportParams);
            var exportOptionsPageViewModel = new ExportOptionsPageViewModel(exportParams);
            var savedFilesViewModel = new SavedFilesViewModel
            {
                ExportParameters = exportParams
            };

            var exportSettings = exportParams.Settings.FirstOrDefault(setting => setting is CsvExportSettings);
            exportSettings.IncludeEvents = true;
            exportSettings.IsActive = true;
            exportSettings.OutputFileHandling = OutputFileRule.CombinedIntoSingleFile;

            exportOptionsPageViewModel.CsvExportOptions.WriteSettings();

            Directory.CreateDirectory(folder);

            savedFilesViewModel.PerformExport(folder);

            Assert.AreEqual(2, savedFilesViewModel.Files.Count);

            Console.WriteLine($"Validating {exportOptionsPageViewModel.CsvExportOptions.ExportFilenames.Count} filename exports");
            foreach (var filename in exportOptionsPageViewModel.CsvExportOptions.ExportFilenames)
            {
                Console.WriteLine($"Validating {filename.RawFilename} and {filename.RawFilename}");
                Assert.IsTrue(File.Exists(Path.Combine(folder, filename.RawFilename)), $"{filename.RawFilename} does not exist");
                Assert.IsTrue(File.Exists(Path.Combine(folder, filename.EventsFilename)), $"{filename.RawFilename} does not exist");
            }
        }

        [TestMethod]
        public void PerformExport_SingleReport_OneFilePerNight_IncludEvents_CustomFilenames()
        {
            var exportParams = new ExportParameters();
            var source = TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH);

            var selectNightsViewModel = new SelectNightsViewModel(exportParams);

            string folder = exportParams.DestinationPath = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString());
            this.tempFolder.Add(folder);

            selectNightsViewModel.LoadFromFolder(source, true);

            var selectSignalsViewModel = new SelectSignalsViewModel(exportParams);
            var exportOptionsPageViewModel = new ExportOptionsPageViewModel(exportParams);
            var savedFilesViewModel = new SavedFilesViewModel
            {
                ExportParameters = exportParams
            };

            var exportSettings = exportParams.Settings.FirstOrDefault(setting => setting is CsvExportSettings);
            exportSettings.IncludeEvents = true;
            exportSettings.IsActive = true;
            exportSettings.OutputFileHandling = OutputFileRule.CombinedIntoSingleFile;

            exportOptionsPageViewModel.CsvExportOptions.WriteSettings();

            exportOptionsPageViewModel.CsvExportOptions.ExportFilenames = [];
            exportOptionsPageViewModel.CsvExportOptions.ExportFilenames.Add(new() {
                RawFilename = Path.Combine(folder, $"Raw Data {Guid.NewGuid()}.csv"),
                EventsFilename = Path.Combine(folder, $"Events Data {Guid.NewGuid()}.csv"),
                Label = "Test File"
            });

            Directory.CreateDirectory(folder);

            savedFilesViewModel.PerformExport(folder);

            Assert.AreEqual(2, savedFilesViewModel.Files.Count);

            foreach (var file in savedFilesViewModel.Files)
            {
                Console.WriteLine(file.Filename);
            }


            Console.WriteLine($"Validating {exportOptionsPageViewModel.CsvExportOptions.ExportFilenames.Count} filename exports");
            foreach (var filename in exportOptionsPageViewModel.CsvExportOptions.ExportFilenames)
            {
                Console.WriteLine($"Validating {filename.RawFilename} and {filename.RawFilename}");
                Assert.IsTrue(File.Exists(Path.Combine(folder, filename.RawFilename)), $"{filename.RawFilename} does not exist");
                Assert.IsTrue(File.Exists(Path.Combine(folder, filename.EventsFilename)), $"{filename.RawFilename} does not exist");
            }
        }

        [TestCleanup]
        public void DeleteTemporaryFiles()
        {
            foreach (string folder in this.tempFolder)
            {
                var files = Directory.GetFiles(folder);

                foreach (string file in files)
                {
                    File.Delete(file);
                }

                Directory.Delete(folder, true);
            }
        }
    }
}
