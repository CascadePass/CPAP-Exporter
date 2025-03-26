using CascadePass.CPAPExporter.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter.Integration.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class SavedFilesViewModelTests
    {
        [TestMethod]
        public void PerformExport_SingleReport_OneFilePerNight_IncludEvents()
        {
            var exportParams = new ExportParameters();
            var source = TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH);

            var selectNightsViewModel = new SelectNightsViewModel(exportParams);

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

            exportOptionsPageViewModel.CsvExportOptions.WriteSettings();
            exportOptionsPageViewModel.CsvExportOptions.ExportFilenames.Add(new() {
                RawFilename = $"Raw Data {Guid.NewGuid()}",
                EventsFilename = $"Events File {Guid.NewGuid()}",
                Label = "Test File"
            });

            string folder = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString());
            Directory.CreateDirectory(folder);

            savedFilesViewModel.PerformExport(folder);

            var files = Directory.GetFiles(folder);

            if (files.Length != 2)
            {
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }

                Directory.Delete(folder, true);
            }

            Assert.AreEqual(2, files.Length);

            Directory.Delete(folder, true);
        }
    }
}
