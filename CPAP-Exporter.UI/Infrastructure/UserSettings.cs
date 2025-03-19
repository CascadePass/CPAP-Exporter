using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CascadePass.CPAPExporter
{
    public class UserSettings
    {
        private static string filename;

        public UserSettings() {
            this.RecentlyUsedFolders = new();
        }

        public int WindowX { get; set; }
        public int WindowY { get; set; }

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        public WindowState WindowState { get; set; }

        public double FontSize { get; set; }

        public string Theme { get; set; }

        public List<string> RecentlyUsedFolders { get; set; }

        public static string Filename
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(UserSettings.filename))
                {
                    return UserSettings.filename;
                }

                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appName = Assembly.GetExecutingAssembly().GetName().Name;
                string settingsFilePath = Path.Combine(appDataPath, appName, "UserSettings.json");

                return UserSettings.filename = settingsFilePath;
            }
        }

        public void Save()
        {
            UserSettings.CreateSubFolder();

            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(UserSettings.Filename, json);
        }

        public static UserSettings Load()
        {
            UserSettings.CreateSubFolder();

            if (File.Exists(UserSettings.Filename))
            {
                var json = File.ReadAllText(UserSettings.Filename);
                var settings = JsonConvert.DeserializeObject<UserSettings>(json);

                return settings;
            }

            return new();
        }

        internal static void CreateSubFolder()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appName = Assembly.GetExecutingAssembly().GetName().Name;
            string appSubFolder = Path.Combine(appDataPath, appName);

            if (!Directory.Exists(appSubFolder))
            {
                Directory.CreateDirectory(appSubFolder);
            }
        }
    }
}
