namespace CascadePass.CPAPExporter.Integration.Tests
{
    //[TestClass]
    public class SelectSignalsViewModelTests
    {
        [TestMethod]
        public void Signals_AirSense_11_APAP()
        {
            var exportParams = new ExportParameters();
            var source = TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH);

            var nightsViewModel = new SelectNightsViewModel(exportParams);

            nightsViewModel.LoadFromFolder(source, true);
            var viewModel = new SelectSignalsViewModel(exportParams);

            Assert.IsTrue(viewModel.Signals.Count > 0, "There are no signals.");
        }

        [TestMethod]
        public void Reports_AirSense_11_APAP()
        {
            var exportParams = new ExportParameters();
            var source = TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH);

            var viewModel = new SelectSignalsViewModel(exportParams);

            Assert.IsFalse(viewModel.Reports.Any(report => report.DailyReport.Sessions.Any(session => session.Signals.Count > 0)), "There should be no signals yet.");

            var nightsViewModel = new SelectNightsViewModel(exportParams);
            nightsViewModel.LoadFromFolder(source, true);

            Assert.IsTrue(nightsViewModel.Reports.Any(report => report.DailyReport.Sessions.Any(session => session.Signals.Count > 0)), "There are no signals.");
        }
    }
}
