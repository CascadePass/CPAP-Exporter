using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class SelectNightsViewModelTests
    {
        #region Title/Description Validation

        [TestMethod]
        public void HasCorrectTitle()
        {
            var vm = new SelectNightsViewModel();
            Assert.AreEqual(Resources.PageTitle_SelectNights, vm.Title);
        }

        [TestMethod]
        public void HasCorrectDescription()
        {
            var vm = new SelectNightsViewModel();
            Assert.AreEqual(Resources.PageDesc_SelectNights, vm.PageDescription);
        }

        #endregion

        [TestMethod]
        public void IsAllSelected_True()
        {
            var exportParameters = new ExportParameters
            {
                Reports =
                    [
                        new DailyReportViewModel(new(), false),
                        new DailyReportViewModel(new(), false),
                        new DailyReportViewModel(new(), false),
                    ]
            };

            var selectNightsViewModel = new SelectNightsViewModel
            {
                ExportParameters = exportParameters,
            };

            Assert.IsFalse(selectNightsViewModel.Reports.Any(r => r.IsSelected));

            selectNightsViewModel.IsAllSelected = true;
            Assert.IsFalse(selectNightsViewModel.Reports.Any(r => !r.IsSelected));
        }

        [TestMethod]
        public void IsAllSelected_False()
        {
            var exportParameters = new ExportParameters();
            exportParameters.Reports =
                [
                    new DailyReportViewModel(new(), true),
                    new DailyReportViewModel(new(), true),
                    new DailyReportViewModel(new(), true),
                ];

            var selectNightsViewModel = new SelectNightsViewModel
            {
                ExportParameters = exportParameters,

                // IsAllSelected defaults to false, because it's boolean,
                // and all setters only do anything when the value changes.
                // So this *has to* be set to true before the test can run.
                IsAllSelected = true,
            };

            Assert.IsFalse(selectNightsViewModel.Reports.Any(r => !r.IsSelected));

            selectNightsViewModel.IsAllSelected = false;
            Assert.IsFalse(selectNightsViewModel.Reports.Any(r => r.IsSelected));
        }
    }
}
