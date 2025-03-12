using cpaplib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class DailyReportsViewModelTests
    {
        [TestMethod]
        public void LoadFromFolder_InvalidDrive()
        {
            var viewModel = new SelectNightsViewModel() { ExportParameters = new() };
            viewModel.LoadFromFolder("Z:\\", true);

            Assert.IsTrue(viewModel.Reports.Count == 0);
        }

        [TestMethod]
        public void LoadFromFolder_InvalidFolder()
        {
            var viewModel = new SelectNightsViewModel() { ExportParameters = new() };
            viewModel.LoadFromFolder(Environment.GetFolderPath(Environment.SpecialFolder.Startup), true);

            Assert.IsTrue(viewModel.Reports.Count == 0);
        }

        [TestMethod]
        public void ReportsNotClearedWhenCantLoad()
        {
            var viewModel = new SelectNightsViewModel() { ExportParameters = new() };

            viewModel.Reports.Add(new DailyReportViewModel() { DailyReport = new() });
            viewModel.Reports.Add(new DailyReportViewModel() { DailyReport = new() });
            viewModel.Reports.Add(new DailyReportViewModel() { DailyReport = new() });

            viewModel.LoadFromFolder("Z:\\", true);

            Assert.IsTrue(viewModel.Reports.Count == 3);
        }
    }
}
