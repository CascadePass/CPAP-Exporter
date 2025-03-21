namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class OpenFilesViewModelTests
    {
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
