using cpaplib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter.Core.Tests
{
    [TestClass]
    public class ExportDetailsTests
    {
        #region Enforce validity (of the daily reports)

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullReports()
        {
            _ = new ExportDetails(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyReportsList()
        {
            _ = new ExportDetails([]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ReportsWithNoSessions()
        {
            List<DailyReport> reports = [];
            DailyReport dailyReport = new();
            reports.Add(dailyReport);

            var exportDetails = new ExportDetails(reports);
        }

        #endregion
    }
}
