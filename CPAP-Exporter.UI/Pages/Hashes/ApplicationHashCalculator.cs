using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace CascadePass.CPAPExporter
{
    public static class ApplicationHashCalculator
    {
        public static Dictionary<string, string> CalculateHashes()
        {
            return ApplicationHashCalculator.CalculateHashes(false);
        }

        public static Dictionary<string, string> CalculateHashes(bool includeSystemModules)
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

                    if (!includeSystemModules && IsMicrosoftDll(modulePath))
                    {
                        continue;
                    }

                    if (!hashes.ContainsKey(modulePath))
                    {
                        hashes[modulePath] = ComputeSHA256Hash(modulePath);
                    }
                }
            }
            catch (Exception)
            {
            }

            return hashes
                .OrderBy(hash => IsMicrosoftDll(hash.Key))
                .ToDictionary(hash => hash.Key, hash => hash.Value);
        }

        internal static bool IsMicrosoftDll(string filePath)
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

        internal static string ComputeSHA256Hash(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            try
            {
                using FileStream stream = File.OpenRead(filePath);
                using SHA256 sha256 = SHA256.Create();
                byte[] hash = sha256.ComputeHash(stream);
                return Convert.ToHexStringLower(hash);
            }
            catch (Exception)
            {
                // File was locked for reading?
                return new string('0', 32);
            }
        }
    }
}