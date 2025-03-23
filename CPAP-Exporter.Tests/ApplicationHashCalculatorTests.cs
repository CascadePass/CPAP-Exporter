using System.Runtime.Intrinsics.Arm;
using System.Windows;
using System.Windows.Input;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class ApplicationHashCalculatorUnitTests
    {
        [TestMethod]
        public void CalculateHashes_ShouldReturnValidHashes()
        {
            var hashes = ApplicationHashCalculator.CalculateHashes(false);

            Assert.IsNotNull(hashes, "Hashes dictionary should not be null");
            Assert.IsTrue(hashes.Count > 0, "Hashes dictionary should contain at least one entry");

            foreach (var hash in hashes)
            {
                Assert.IsFalse(string.IsNullOrEmpty(hash.Key), "File path should not be null or empty");
                Assert.IsFalse(string.IsNullOrEmpty(hash.Value), "Hash value should not be null or empty");

                Assert.AreEqual(32 * 2, hash.Value.Length, $"Value size was {hash.Value.Length}");
            }
        }

        [TestMethod]
        public void CalculateHashes_ShouldReturnUIProject()
        {
            var hashes = ApplicationHashCalculator.CalculateHashes(false);

            Assert.IsNotNull(hashes, "Hashes dictionary should not be null");
            Assert.IsTrue(hashes.Count > 0, "Hashes dictionary should contain at least one entry");

            Assert.IsTrue(hashes.Keys.Any(key => key.EndsWith("\\net9.0-windows\\CPAP-Exporter.UI.dll")));
        }

        [TestMethod]
        public void CalculateHashes_ExcludeSystem_ShouldNotIncludeWindows()
        {
            var hashes = ApplicationHashCalculator.CalculateHashes(false);

            Assert.IsNotNull(hashes, "Hashes dictionary should not be null");
            Assert.IsTrue(hashes.Count > 0, "Hashes dictionary should contain at least one entry");

            Assert.IsTrue(hashes.Keys.Any(key => key.EndsWith("\\net9.0-windows\\CPAP-Exporter.UI.dll")));
            this.AssertSystemModules(false, hashes);
        }

        [TestMethod]
        public void CalculateHashes_IncludeSystem_ShouldIncludeWindows()
        {
            var hashes = ApplicationHashCalculator.CalculateHashes(true);

            Assert.IsNotNull(hashes, "Hashes dictionary should not be null");
            Assert.IsTrue(hashes.Count > 0, "Hashes dictionary should contain at least one entry");

            Assert.IsTrue(hashes.Keys.Any(key => key.EndsWith("\\net9.0-windows\\CPAP-Exporter.UI.dll")));
            this.AssertSystemModules(true, hashes);
        }

        [TestMethod]
        public void IsMicrosoftDll_ShouldIdentifyMicrosoftDlls()
        {
            string microsoftFilePath = @"C:\Windows\System32\kernel32.dll";
            string nonMicrosoftFilePath = @"C:\ThirdParty\library.dll";


            bool isMicrosoftDll = ApplicationHashCalculator.IsMicrosoftDll(microsoftFilePath);
            bool isNonMicrosoftDll = ApplicationHashCalculator.IsMicrosoftDll(nonMicrosoftFilePath);

            Assert.IsTrue(isMicrosoftDll, "Kernel32.dll should be identified as a Microsoft DLL");
            Assert.IsFalse(isNonMicrosoftDll, "Third-party library should not be identified as a Microsoft DLL");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ComputeSHA256Hash_ShouldThrowExceptionForInvalidFile()
        {
            string invalidFilePath = @"C:\Invalid\NonExistentFile.dll";

            ApplicationHashCalculator.ComputeSHA256Hash(invalidFilePath);

            // Expecting exception to be thrown
        }

        [TestMethod]
        public void ComputeSHA256Hash_ShouldReturnValidHashForValidFile()
        {
            string validFilePath = @"C:\Windows\System32\kernel32.dll";

            string hash = ApplicationHashCalculator.ComputeSHA256Hash(validFilePath);

            Assert.IsFalse(string.IsNullOrEmpty(hash), "Hash should not be null or empty");
        }

        private void AssertSystemModules(bool expectedValue, Dictionary<string, string> hashes)
        {
            string[] files = { "\\ntdll.dll", "\\KERNEL32.DLL", "\\KERNELBASE.dll", "\\SHELL32.dll", "\\System32\\USER32.dll", };

            foreach (string path in files)
            {
                Assert.AreEqual(
                    expectedValue,
                    hashes.Keys.Any(key => key.EndsWith(path)),
                    $"Dictionary {(expectedValue ? "should" : "shouldn't")} contain '{path}'"
                );
            }
        }
    }
}