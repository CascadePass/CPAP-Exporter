using cpaplib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        #region IsAllSelected

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

        #endregion

        #region AddReport

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddReport_null()
        {
            var selectNightsViewModel = new SelectNightsViewModel(new());

            selectNightsViewModel.AddReport(null);
        }

        [TestMethod]
        public void AddReport_ValidReport_ReturnsWrappedViewModel()
        {
            var selectNightsViewModel = new SelectNightsViewModel(new());
            DailyReport dailyReport = new();
            var result = selectNightsViewModel.AddReport(dailyReport);

            Assert.IsNotNull(result);
            Assert.AreSame(dailyReport, result.DailyReport);
        }

        [TestMethod]
        public void AddReport_ValidReport_AddsToReports()
        {
            var selectNightsViewModel = new SelectNightsViewModel(new());
            DailyReport dailyReport = new();
            var result = selectNightsViewModel.AddReport(dailyReport);

            Assert.AreEqual(1, selectNightsViewModel.Reports.Count);
            Assert.IsTrue(selectNightsViewModel.Reports.Any(r => r.DailyReport == dailyReport));
        }

        #endregion

        #region Validate

        [TestMethod]
        public void Validate_NothingChecked()
        {
            var selectNightsViewModel = new SelectNightsViewModel(new());

            for (int i = 0; i < 10; i++)
            {
                var nightViewModel = selectNightsViewModel.AddReport(new());
                nightViewModel.IsSelected = false;
            }

            var result = selectNightsViewModel.Validate();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Validate_EverythingChecked()
        {
            var selectNightsViewModel = new SelectNightsViewModel(new());

            for (int i = 0; i < 10; i++)
            {
                var nightViewModel = selectNightsViewModel.AddReport(new());
                nightViewModel.IsSelected = true;
            }

            var result = selectNightsViewModel.Validate();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Validate_SomeChecked()
        {
            var selectNightsViewModel = new SelectNightsViewModel(new());

            for (int i = 0; i < 10; i++)
            {
                var nightViewModel = selectNightsViewModel.AddReport(new());
                nightViewModel.IsSelected = i % 2 == 0;
            }

            var result = selectNightsViewModel.Validate();

            Assert.IsTrue(result);
        }

        #endregion
    }
}
