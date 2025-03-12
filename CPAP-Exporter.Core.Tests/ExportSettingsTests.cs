using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CascadePass.CPAPExporter.Core.Tests
{
    [TestClass]
    public class ExportSettingsTests
    {
        [TestMethod]
        public void ExportSettings_Defaults()
        {
            var settings = new ExportSettings();

            Assert.AreEqual(OutputFileRule.OneFilePerNight, settings.OutputFileHandling);

            Assert.IsTrue(settings.IncludeRowNumber);
            Assert.IsTrue(settings.IncludeSessionNumber);
            Assert.IsTrue(settings.IncludeTimestamp);

            Assert.IsFalse(settings.IncludePythonBoilerplate);
        }
    }
}
