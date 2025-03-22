using cpaplib;

namespace CascadePass.CPAPExporter.UI.Tests
{
    public class MockCpapSourceValidator : ICpapSourceValidator
    {
        public MockCpapSourceValidator(ICpapDataLoader desiredLoader, bool isValid)
        {
            this.DesiredLoader = desiredLoader;
            this.IsValid = isValid;
        }

        public ICpapDataLoader DesiredLoader { get; set; }

        public bool IsValid { get; set; }

        public ICpapDataLoader GetLoader(string rootFolder)
        {
            return this.DesiredLoader;
        }

        public bool IsCpapFolderStructure(string rootFolder)
        {
            return this.IsValid;
        }
    }
}
