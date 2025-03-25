namespace CascadePass.CPAPExporter.Integration.Tests
{
    [TestClass]
    public class SelectNightsViewModelTests
    {
        [TestMethod]
        public void LoadFromFolder_AirSense_11_APAP()
        {
            var exportParams = new ExportParameters();
            var viewModel = new SelectNightsViewModel(exportParams);
            var source = TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH);

            viewModel.LoadFromFolder(source, true);

            Assert.AreEqual(1, exportParams.Reports.Count);
            Assert.AreEqual(new DateTime(2024, 10, 17), exportParams.Reports[0].DailyReport.ReportDate);

            Assert.IsFalse(viewModel.IsBusy);
            Assert.IsTrue(viewModel.SourceFolders.Any(f => f.Key == source));
        }

        [TestMethod]
        public void ClearReportsBeforeAdding_True()
        {
            var exportParams = new ExportParameters();
            var viewModel = new SelectNightsViewModel(exportParams);
            var source = TestFilePaths.GetEffectivePath(TestFilePaths.AC10_ROOT_PATH);
            var fakeReport = new DailyReportViewModel();

            exportParams.Reports.Add(fakeReport);

            viewModel.ClearReportsBeforeAdding = true;
            viewModel.LoadFromFolder(source, true);

            Assert.AreEqual(1, exportParams.Reports.Count);
            Assert.AreEqual(new DateTime(2025, 02, 02), exportParams.Reports[0].DailyReport.ReportDate);

            Assert.IsFalse(viewModel.IsBusy);
            Assert.IsTrue(viewModel.SourceFolders.Any(f => f.Key == source));

            Assert.IsFalse(viewModel.Reports.Any(r => r == fakeReport));
        }

        [TestMethod]
        public void ClearReportsBeforeAdding_false()
        {
            var exportParams = new ExportParameters();
            var viewModel = new SelectNightsViewModel(exportParams);
            var source = TestFilePaths.GetEffectivePath(TestFilePaths.AirBreak_AS10_ASV_ROOT_PATH);
            var fakeReport = new DailyReportViewModel();

            exportParams.Reports.Add(fakeReport);

            viewModel.ClearReportsBeforeAdding = false;
            viewModel.LoadFromFolder(source, false);

            Assert.AreEqual(2, exportParams.Reports.Count);
            Assert.AreEqual(new DateTime(2025, 02, 11), exportParams.Reports[1].DailyReport.ReportDate);

            Assert.IsFalse(viewModel.IsBusy);
            Assert.IsTrue(viewModel.SourceFolders.Any(f => f.Key == source));

            Assert.IsTrue(viewModel.Reports.Any(r => r == fakeReport));
        }
    }
}
