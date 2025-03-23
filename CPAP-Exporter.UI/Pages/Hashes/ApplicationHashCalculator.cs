using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace CascadePass.CPAPExporter
{
    public class ApplicationHashCalculator
    {
        public static Dictionary<string, string> CalculateHashes()
        {
            var hashes = new Dictionary<string, string>();
            try
            {
                // Get the main module (the EXE of the running application)
                Process process = Process.GetCurrentProcess();
                string mainModulePath = process.MainModule.FileName;
                hashes[mainModulePath] = ComputeSHA256Hash(mainModulePath);

                // Get all loaded modules (DLLs)
                var loadedModules = process.Modules.Cast<ProcessModule>()
                    .Where(m => m.ModuleName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));

                foreach (var module in loadedModules)
                {
                    string modulePath = module.FileName;

                    // Exclude Microsoft DLLs
                    if (IsMicrosoftDll(modulePath))
                    {
                        continue;
                    }

                    if (!hashes.ContainsKey(modulePath))
                    {
                        hashes[modulePath] = ComputeSHA256Hash(modulePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating hashes: {ex.Message}");
            }

            return hashes;
        }

        private static bool IsMicrosoftDll(string filePath)
        {
            string lowerFilePath = filePath.ToLowerInvariant();

            // Check for common Microsoft paths and names
            return lowerFilePath.Contains("windows\\system32") ||
                   lowerFilePath.Contains("microsoft shared") ||
                   lowerFilePath.Contains("\\microsoft\\") ||
                   lowerFilePath.Contains("\\microsoft") ||
                   lowerFilePath.Contains("microsoft\\") ||
                   lowerFilePath.Contains("\\microsoft.net\\")
                   ;
        }

        private static string ComputeSHA256Hash(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}