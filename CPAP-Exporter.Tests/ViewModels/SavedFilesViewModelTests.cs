using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class SavedFilesViewModelTests
    {
        #region Title/Description Validation

        [TestMethod]
        public void HasCorrectTitle()
        {
            var vm = new SavedFilesViewModel();
            Assert.AreEqual(Resources.PageTitle_SavedFiles, vm.Title);
        }

        [TestMethod]
        public void HasCorrectDescription()
        {
            var vm = new SavedFilesViewModel();
            Assert.AreEqual(Resources.PageDesc_SavedFiles, vm.PageDescription);
        }

        #endregion

        [TestMethod]
        public void Files_NotNullOnNewInstance()
        {
            var vm = new SavedFilesViewModel();
            Assert.IsNotNull(vm.Files);
        }

        [TestMethod]
        public void AddFile_NonExistent()
        {
            var vm = new SavedFilesViewModel();

            string badFilename = Guid.NewGuid().ToString();
            vm.AddFile(badFilename, "Test File", SavedFileType.FullExport);

            Assert.IsFalse(vm.Files.Any(f => f.Filename.Contains(badFilename)));
        }

        [TestMethod]
        public void AddFile_RealFile()
        {
            var vm = new SavedFilesViewModel();

            string filename = Guid.NewGuid().ToString();
            File.WriteAllText(filename, filename);

            vm.AddFile(filename, "Test File", SavedFileType.FullExport);

            Assert.IsTrue(vm.Files.Any(f => f.Filename.Contains(filename)));
            File.Delete(filename);
        }

        [TestMethod]
        public void RemovesDeletedFile()
        {
            string filename = Guid.NewGuid().ToString();
            File.WriteAllText(filename, filename);

            var vm = new SavedFilesViewModel();

            SavedFileViewModel deletedFile = vm.AddFile(filename, "Test File", SavedFileType.FullExport);

            deletedFile.DeleteFile();

            Assert.IsFalse(vm.Files.Contains(deletedFile), "Deleted file was not removed.");

            // There's no need to validate that the file was deleted, because
            // SavedFileViewModelTests has a test that checks this.
        }
    }
}
