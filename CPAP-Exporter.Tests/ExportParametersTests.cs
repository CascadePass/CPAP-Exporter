namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class ExportParametersTests
    {
        [TestMethod]
        public void Constructor_PropertiesAreReadyToUse()
        {
            ExportParameters exportParameters = new();

            Assert.IsNotNull(exportParameters.Reports);
            Assert.IsNotNull(exportParameters.Signals);
            Assert.IsNotNull(exportParameters.SignalNames);
        }

        [TestMethod]
        public void SignalNames_OnlySelectedSignals()
        {
            ExportParameters exportParameters = new();

            exportParameters.Signals.Add(new SignalViewModel(new() { Name = "Signal 1" }) { IsSelected = true });
            exportParameters.Signals.Add(new SignalViewModel(new() { Name = "Signal 2" }) { IsSelected = true });
            exportParameters.Signals.Add(new SignalViewModel(new() { Name = "Signal 3" }) { IsSelected = false });
            exportParameters.Signals.Add(new SignalViewModel(new() { Name = "Signal 4" }) { IsSelected = false });
                                                             
            Assert.AreEqual(2, exportParameters.SignalNames.Count);
                                                             
            Assert.IsTrue(exportParameters.SignalNames.Contains("Signal 1"));
            Assert.IsTrue(exportParameters.SignalNames.Contains("Signal 2"));
            Assert.IsFalse(exportParameters.SignalNames.Contains("Signal 3"));
            Assert.IsFalse(exportParameters.SignalNames.Contains("Signal 4"));
        }
    }
}
