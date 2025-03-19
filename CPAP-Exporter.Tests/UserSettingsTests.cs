namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class UserSettingsTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeRecentlyUsedFolders()
        {
            var settings = new UserSettings();

            Assert.IsNotNull(settings.RecentlyUsedFolders);
            Assert.AreEqual(0, settings.RecentlyUsedFolders.Count);
        }

        [TestMethod]
        public void Filename_ShouldReturnNonEmptyPath()
        {
            var filename = UserSettings.Filename;

            Assert.IsFalse(string.IsNullOrWhiteSpace(filename));
            StringAssert.EndsWith(filename, "UserSettings.json");
        }

        [TestMethod]
        public void Save_ShouldCreateFile()
        {
            var settings = new UserSettings
            {
                WindowX = 100,
                WindowY = 200,
                WindowWidth = 800,
                WindowHeight = 600,
                FontSize = 14,
                Theme = "Dark",
                RecentlyUsedFolders = new List<string> { "Folder1", "Folder2" }
            };

            settings.Save();

            Assert.IsTrue(File.Exists(UserSettings.Filename));

            File.Delete(UserSettings.Filename);
        }

        [TestMethod]
        public void Load_ShouldReturnDefaultSettings_WhenFileDoesNotExist()
        {
            if (File.Exists(UserSettings.Filename))
            {
                File.Delete(UserSettings.Filename);
            }

            var settings = UserSettings.Load();

            Assert.IsNotNull(settings);
            Assert.AreEqual(0, settings.RecentlyUsedFolders.Count);
        }

        [TestMethod]
        public void Load_ShouldReturnSavedSettings_WhenFileExists()
        {
            var originalSettings = new UserSettings
            {
                WindowX = 50,
                WindowY = 100,
                FontSize = 12.5,
                Theme = "Light"
            };
            originalSettings.Save();

            var loadedSettings = UserSettings.Load();

            Assert.IsNotNull(loadedSettings);
            Assert.AreEqual(originalSettings.WindowX, loadedSettings.WindowX);
            Assert.AreEqual(originalSettings.Theme, loadedSettings.Theme);

            File.Delete(UserSettings.Filename);
        }
    }
}