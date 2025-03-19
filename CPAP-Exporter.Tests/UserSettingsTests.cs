using System.Reflection;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    [DoNotParallelize]
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

            Console.WriteLine(filename);
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

        [TestMethod]
        public void CreateSubFolder_DoesNotExist()
        {
            string filename = UserSettings.Filename;

            string appSubFolder = Path.GetDirectoryName(filename);

            if (Directory.Exists(appSubFolder))
            {
                Directory.Delete(appSubFolder, true);
            }

            UserSettings.CreateSubFolder();

            Assert.IsTrue(Directory.Exists(appSubFolder));
        }

        [TestMethod]
        public void CreateSubFolder_Exists()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appName = Assembly.GetExecutingAssembly().GetName().Name;
            string appSubFolder = Path.Combine(appDataPath, appName);

            if (!Directory.Exists(appSubFolder))
            {
                Directory.CreateDirectory(appSubFolder);
            }

            Assert.IsTrue(Directory.Exists(appSubFolder));

            UserSettings.CreateSubFolder();

            Assert.IsTrue(Directory.Exists(appSubFolder));

            // Expectation is that no exception is thrown when
            // creating a folder that already exists.
        }

        #region AddFolder

        [TestMethod]
        public void AddFolder_AddsNewFolder()
        {
            var settings = new UserSettings();

            settings.AddFolder("Folder1");
            Assert.IsTrue(settings.RecentlyUsedFolders.Contains("Folder1"));
        }

        [TestMethod]
        public void AddFolder_DoesNotAddDuplicateFolder()
        {
            var settings = new UserSettings();

            settings.AddFolder("Folder1");
            settings.AddFolder("Folder1"); // Duplicate

            Assert.AreEqual(1, settings.RecentlyUsedFolders.Count);
        }

        [TestMethod]
        public void AddFolder_RemovesOldestFolderWhenLimitExceeded()
        {
            var settings = new UserSettings();

            for (int i = 0; i < 20; i++)
            {
                settings.AddFolder($"Folder{i}");
            }

            Assert.IsFalse(settings.RecentlyUsedFolders.Contains("Folder1"));
            Assert.AreEqual(15, settings.RecentlyUsedFolders.Count); // Stays at limit
        }

        [TestMethod]
        public void AddFolder_KeepsFoldersInOrder()
        {
            var settings = new UserSettings();

            settings.AddFolder("Folder1");
            settings.AddFolder("Folder2");
            settings.AddFolder("Folder3");

            CollectionAssert.AreEqual(
                new List<string> { "Folder1", "Folder2", "Folder3" },
                settings.RecentlyUsedFolders
            );

            CollectionAssert.AreEqual(
                new List<string> { "Folder1", "Folder2", "Folder3" },
                settings.RecentlyUsedFolders
            );
        }

        #endregion
    }
}