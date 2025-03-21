using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
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

        [TestMethod]
        public void CanImportFrom_WithNonExistentFolder_ShouldReturnFalse()
        {
            var folderPath = @"C:\NonExistentFolder";
            var viewModel = new OpenFilesViewModel();

            Assert.IsFalse(viewModel.CanImportFrom(folderPath));
        }

        //TODO: Add real machine data
    }
}
