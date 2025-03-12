namespace CascadePass.CPAPExporter.Core.Tests
{
    [TestClass]
    public class CsvExportSettingsTests
    {
        [TestMethod]
        public void CsvExportSettings_Defaults()
        {
            var settings = new CsvExportSettings();

            Assert.IsTrue(settings.IncludeColumnHeaders);
            Assert.AreEqual(",", settings.Delimiter);
        }

        [TestMethod]
        public void CsvExportSettings_InheritedDefaults()
        {
            var settings = new CsvExportSettings();

            Assert.AreEqual(OutputFileRule.OneFilePerNight, settings.OutputFileHandling);

            Assert.IsTrue(settings.IncludeRowNumber);
            Assert.IsTrue(settings.IncludeSessionNumber);
            Assert.IsTrue(settings.IncludeTimestamp);

            Assert.IsFalse(settings.IncludePythonBoilerplate);
        }
    }
}
