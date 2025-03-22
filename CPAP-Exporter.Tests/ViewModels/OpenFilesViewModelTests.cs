namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class OpenFilesViewModelTests
    {
        #region Title/Description Validation

        [TestMethod]
        public void HasCorrectTitle()
        {
            var vm = new OpenFilesViewModel();
            Assert.AreEqual(Resources.PageTitle_OpenFiles, vm.Title);
        }

        [TestMethod]
        public void HasCorrectDescription()
        {
            var vm = new OpenFilesViewModel();
            Assert.AreEqual(Resources.PageDesc_OpenFiles, vm.PageDescription);
        }

        #endregion

        [TestMethod]
        public void Validate_FalseBeforeSelectingFolder()
        {
            var viewModel = new OpenFilesViewModel();

            Assert.IsFalse(viewModel.Validate());
            Assert.IsFalse(viewModel.IsValid);
        }

        #region CanImportFrom

        [TestMethod]
        public void CanImportFrom_WithNonExistentFolder_ShouldReturnFalse()
        {
            var folderPath = @"C:\NonExistentFolder";
            var viewModel = new OpenFilesViewModel();

            Assert.IsFalse(viewModel.CanImportFrom(folderPath));
        }

        [TestMethod]
        public void CanImportFrom_Whitespace_ShouldReturnFalse()
        {
            var viewModel = new OpenFilesViewModel();

            Assert.IsFalse(viewModel.CanImportFrom(" "));
        }

        [TestMethod]
        public void CanImportFrom_Null_ShouldReturnFalse()
        {
            var viewModel = new OpenFilesViewModel();

            Assert.IsFalse(viewModel.CanImportFrom(" "));
        }

        #endregion

        #region FindImportableParentFolder

        [TestMethod]
        public void FindImportableParentFolder_ValidFolder()
        {
            var folderPath = @"C:\PAP Data\Machine";
            var viewModel = new OpenFilesViewModel();

            ApplicationComponentProvider.CpapSourceValidator = new MockCpapSourceValidator(null, true);

            var result = viewModel.FindImportableParentFolder(folderPath);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [TestMethod]
        public void FindImportableParentFolder_InvalidFolder()
        {
            var folderPath = @"C:\PAP Data\Machine";
            var viewModel = new OpenFilesViewModel();

            ApplicationComponentProvider.CpapSourceValidator = new MockCpapSourceValidator(null, false);

            var result = viewModel.FindImportableParentFolder(folderPath);

            Assert.IsTrue(string.IsNullOrWhiteSpace(result));
        }

        #endregion
    }
}
