using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter.Integration.Tests
{
    [TestClass]
    public class SavedFilesViewModelTests
    {
        [TestMethod]
        public void ExportSingleReport()
        {
            var exportParams = new ExportParameters();
            var source = TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH);

            var selectNightsViewModel = new SelectNightsViewModel(exportParams);
            var selectSignalsViewModel = new SelectSignalsViewModel(exportParams);
            var exportOptionsPageViewModel = new ExportOptionsPageViewModel(exportParams);
            var savedFilesViewModel = new SavedFilesViewModel();

            selectNightsViewModel.LoadFromFolder(source, true);

            string folder = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString());
            Directory.CreateDirectory(folder);

            savedFilesViewModel.PerformExport(folder);

            var files = Directory.GetFiles(folder);
            Assert.AreEqual(2, files.Length);            

            Directory.Delete(folder, true);
        }
    }
}
