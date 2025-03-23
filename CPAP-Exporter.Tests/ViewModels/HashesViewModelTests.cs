namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class HashesViewModelTests
    {
        #region Title/Description Validation

        [TestMethod]
        public void HasCorrectTitle()
        {
            var vm = new HashesViewModel();
            Assert.AreEqual(Resources.PageTitle_Hashes, vm.Title);
        }

        [TestMethod]
        public void HasCorrectDescription()
        {
            var vm = new HashesViewModel();
            Assert.AreEqual(Resources.PageDesc_Hashes, vm.PageDescription);
        }

        #endregion

        [TestMethod]
        public void IncludeSystemModules_FalseBeforeSelectingFolder()
        {
            var viewModel = new HashesViewModel();

            Assert.IsFalse(viewModel.IncludeSystemModules);
        }


        [TestMethod]
        public void IncludeSystemModules_FileHashes()
        {
            var viewModel = new HashesViewModel();

            // Need to call .ToArray() because FileHashes is an ObservableCollection that gets cleared
            var result = viewModel.FileHashes.ToArray();

            viewModel.IncludeSystemModules = !viewModel.IncludeSystemModules;

            // Need to call .ToArray() because FileHashes is an ObservableCollection that gets cleared
            var result2 = viewModel.FileHashes.ToArray();

            Assert.AreNotEqual(result.Length, result2.Length);
        }
    }
}
