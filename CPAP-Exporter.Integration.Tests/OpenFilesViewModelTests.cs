namespace CascadePass.CPAPExporter.Integration.Tests
{
    [TestClass]
    public class OpenFilesViewModelTests
    {
        #region CanImportFrom

        [TestMethod]
        public void CanImportFrom_AirSense_11_APAP()
        {
            string sourcePath = TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH);
            var viewModel = new OpenFilesViewModel(new());

            Assert.IsTrue(viewModel.CanImportFrom(sourcePath));

            //viewModel.Load(sourcePath);

            //Assert.AreEqual(1, parameters.Reports.Count, $"Found {parameters.Reports.Count} reports in {sourcePath}");
        }

        [TestMethod]
        public void CanImportFrom_AirCurve_10_VAuto()
        {
            string sourcePath = TestFilePaths.GetEffectivePath(TestFilePaths.AC10_ROOT_PATH);
            var viewModel = new OpenFilesViewModel(new());

            Assert.IsTrue(viewModel.CanImportFrom(sourcePath));
        }

        [TestMethod]
        public void CanImportFrom_AirBroken_AirSense_10_ASVAuto()
        {
            string sourcePath = TestFilePaths.GetEffectivePath(TestFilePaths.AirBreak_AS10_ASV_ROOT_PATH);
            var viewModel = new OpenFilesViewModel(new());

            Assert.IsTrue(viewModel.CanImportFrom(sourcePath));
        }

        #endregion

        #region FindImportableParentFolder

        [TestMethod]
        public void FindImportableParentFolder_AS11_Night()
        {
            string sourcePath = TestFilePaths.GetEffectivePath(TestFilePaths.AS11_NIGHT_PATH);
            var viewModel = new OpenFilesViewModel(new());

            // The found path should match the expected result:
            Assert.AreEqual(
                TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH),
                viewModel.FindImportableParentFolder(sourcePath)
            );

            // The ViewModel should also consider this importable:
            Assert.IsTrue(viewModel.CanImportFrom(viewModel.FindImportableParentFolder(sourcePath)));
        }

        [TestMethod]
        public void FindImportableParentFolder_AC10_Night()
        {
            string sourcePath = TestFilePaths.GetEffectivePath(TestFilePaths.AC10_NIGHT_PATH);
            var viewModel = new OpenFilesViewModel(new());

            // The found path should match the expected result:
            Assert.AreEqual(
                TestFilePaths.GetEffectivePath(TestFilePaths.AC10_ROOT_PATH),
                viewModel.FindImportableParentFolder(sourcePath)
            );

            // The ViewModel should also consider this importable:
            Assert.IsTrue(viewModel.CanImportFrom(viewModel.FindImportableParentFolder(sourcePath)));
        }

        [TestMethod]
        public void FindImportableParentFolder_AS10_Night()
        {
            string sourcePath = TestFilePaths.GetEffectivePath(TestFilePaths.AirBreak_AS10_ASV_NIGHT_PATH);
            var viewModel = new OpenFilesViewModel(new());

            // The found path should match the expected result:
            Assert.AreEqual(
                TestFilePaths.GetEffectivePath(TestFilePaths.AirBreak_AS10_ASV_ROOT_PATH),
                viewModel.FindImportableParentFolder(sourcePath)
            );

            // The ViewModel should also consider this importable:
            Assert.IsTrue(viewModel.CanImportFrom(viewModel.FindImportableParentFolder(sourcePath)));
        }

        #endregion

        #region Load

        #region ResMed AirSense 11 (in APAP)

        [TestMethod]
        public void Load_NonExistantFolder()
        {
            string badFolder = "Z:\\CPAP";
            var exportParams = new ExportParameters();
            var viewModel = new OpenFilesViewModel(exportParams);
            
            viewModel.Load(badFolder);

            Assert.AreNotEqual(badFolder, exportParams.SourcePath);
            Assert.IsFalse(viewModel.IsValid);

        }

        [TestMethod]
        public void Load_AS11_Root()
        {
            var exportParams = new ExportParameters();
            var viewModel = new OpenFilesViewModel(exportParams);

            viewModel.Load(TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH));

            Assert.AreEqual(TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH), exportParams.SourcePath);
            Assert.IsTrue(viewModel.IsValid);
        }

        [TestMethod]
        public void Load_AS11_Night()
        {
            var exportParams = new ExportParameters();
            var viewModel = new OpenFilesViewModel(exportParams);

            viewModel.Load(TestFilePaths.GetEffectivePath(TestFilePaths.AS11_NIGHT_PATH));

            Assert.AreEqual(TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH), exportParams.SourcePath);
            Assert.IsTrue(viewModel.IsValid);
        }

        [TestMethod]
        public void Load_AS11_Settings()
        {
            var exportParams = new ExportParameters();
            var viewModel = new OpenFilesViewModel(exportParams);

            viewModel.Load(TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH + "\\SETTINGS"));

            Assert.AreEqual(TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH), exportParams.SourcePath);
            Assert.IsTrue(viewModel.IsValid);
        }

        [TestMethod]
        public void Load_AS11_DataLog()
        {
            var exportParams = new ExportParameters();
            var viewModel = new OpenFilesViewModel(exportParams);

            viewModel.Load(TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH + "\\DATALOG"));

            Assert.AreEqual(TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH), exportParams.SourcePath);
            Assert.IsTrue(viewModel.IsValid);
        }

        #endregion

        #endregion

        #region ClearReportsBeforeAdding

        // This functionality is machine agnostic.

        [TestMethod]
        public void ClearReportsBeforeAdding_True()
        {
            var badReport = new DailyReportViewModel();
            var exportParams = new ExportParameters();
            var viewModel = new OpenFilesViewModel(exportParams);

            exportParams.Reports.Add(badReport);

            viewModel.ClearReportsBeforeAdding = true;
            viewModel.Load(TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH));

            Assert.AreEqual(0, exportParams.Reports.Count);
            Assert.IsFalse(exportParams.Reports.Contains(badReport));
        }

        [TestMethod]
        public void ClearReportsBeforeAdding_False()
        {
            var badReport = new DailyReportViewModel();
            var exportParams = new ExportParameters();
            var viewModel = new OpenFilesViewModel(exportParams);

            exportParams.Reports.Add(badReport);

            viewModel.ClearReportsBeforeAdding = false;
            viewModel.Load(TestFilePaths.GetEffectivePath(TestFilePaths.AS11_ROOT_PATH));

            Assert.AreEqual(1, exportParams.Reports.Count);
            Assert.IsTrue(exportParams.Reports.Contains(badReport));
        }

        #endregion

        #region Folders

        #endregion
    }
}
