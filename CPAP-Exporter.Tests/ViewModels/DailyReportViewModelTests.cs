using cpaplib;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class DailyReportViewModelTests
    {
        [TestMethod]
        public void PressureDescription_AutoASV()
        {
            DailyReport dailyReport = new();
            DailyReportViewModel dailyReportViewModel = new() { DailyReport = dailyReport };

            dailyReport.Settings["Mode"] = "AsvVariableEpap";
            dailyReport.Settings["PS Minimum"] = "1";
            dailyReport.Settings["PS Maximum"] = "2";
            dailyReport.Settings["EPAP Min"] = "3";
            dailyReport.Settings["EPAP Max"] = "4";

            Assert.AreEqual($"{dailyReport.Settings["PS Minimum"]} - {dailyReport.Settings["PS Maximum"]} Over {dailyReport.Settings["EPAP Min"]} - {dailyReport.Settings["EPAP Max"]}", dailyReportViewModel.PressureDescription);
        }
    }
}
