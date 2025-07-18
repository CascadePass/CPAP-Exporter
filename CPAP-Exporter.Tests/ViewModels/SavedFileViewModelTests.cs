using System.Diagnostics;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class SavedFileViewModelTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_NonExistantFile()
        {
            var savedFileViewModel = new SavedFileViewModel(Guid.NewGuid().ToString(), "Test File", SavedFileType.FullExport);
        }

        [TestMethod]
        public void Constructor_ExistingFile()
        {
            string description = "Test File";
            string filename = Directory.GetFiles(".").First();
            var savedFileViewModel = new SavedFileViewModel(filename, description, SavedFileType.FullExport);

            Assert.AreEqual(filename, savedFileViewModel.Filename);
            Assert.AreEqual(description, savedFileViewModel.Description);
            Assert.AreEqual(new FileInfo(filename).Length, savedFileViewModel.FileInfo.Length);
        }

        [TestMethod]
        public void Delete() {
            string filename = $"{Guid.NewGuid()}.tmp";

            File.WriteAllText(filename, filename);

            SavedFileViewModel savedFileViewModel = new(filename, "Test File", SavedFileType.FullExport);

            savedFileViewModel.DeleteCommand.Execute(null);

            Debug.WriteLine(filename);
            Assert.IsFalse(new FileInfo(filename).Exists, "File was not deleted.");
        }
    }
}
