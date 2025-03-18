using System.ComponentModel;
using System.Diagnostics;

namespace CascadePass.CPAPExporter
{
    public static class WindowsExplorerUtility
    {
        public static bool LaunchFile(string filename)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = filename,
                UseShellExecute = true
            };

            return WindowsExplorerUtility.StartProcess(startInfo);

            //try
            //{
            //    Process.Start(filename);
            //}
            //catch (Win32Exception) { LogException(); return false; }
            //catch (ObjectDisposedException) { LogException(); return false; }
            //catch (InvalidOperationException) { LogException(); return false; }

            //return true;
        }

        public static bool BrowseToFolder(string path)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = path,
                UseShellExecute = true
            };

            return WindowsExplorerUtility.StartProcess(startInfo);
        }

        public static bool BrowseToFile(string filePath)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"/select,\"{filePath}\"",
                UseShellExecute = true
            };

            return WindowsExplorerUtility.StartProcess(startInfo);
        }

        private static bool StartProcess(ProcessStartInfo psi)
        {
            try
            {
                Process.Start(psi);
            }
            catch (Win32Exception) { LogException(); return false; }
            catch (ObjectDisposedException) { LogException(); return false; }
            catch (InvalidOperationException) { LogException(); return false; }

            return true;
        }

        private static void LogException()
        {
        }
    }

}
