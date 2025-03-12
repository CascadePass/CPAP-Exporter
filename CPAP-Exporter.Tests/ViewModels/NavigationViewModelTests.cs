using System.Windows;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class NavigationViewModelTests
    {
        private const string CURRENT_STYLE = "SelectedNavigationButtonStyle";
        private const string DISABLED_STYLE = "DisabledButtonStyle";
        private const string NORMAL_STYLE = "NavigationButtonStyle";

        [TestMethod]
        public void NewInstance_CurrentPageIsWelcomeScreen()
        {
            NavigationViewModel navigationViewModel = new() { PageViewModelProvider = new MockViewProvider(new WelcomeViewModel()) };

            Assert.AreEqual(NavigationStep.Welcome, navigationViewModel.CurrentStep);
        }

        [TestMethod]
        public void NewInstance_HasExportParameters()
        {
            NavigationViewModel navigationViewModel = new();

            Assert.IsNotNull(navigationViewModel.ExportParameters);
        }

        #region Button Styles

        #region Happy Path (CurrentStep vs availability)

        [TestMethod]
        public void Welcome_NavigationOptions()
        {
            NavigationViewModel navigationViewModel = new() {
                CurrentStep = NavigationStep.Welcome,
                PageViewModelProvider = new MockViewProvider(new WelcomeViewModel())
            };

            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        [TestMethod]
        public void SelectNights_NavigationOptions_Invalid()
        {
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.SelectDays,
                PageViewModelProvider = new MockViewProvider(new SelectNightsViewModel() { ValidationProvider = new MockValidator(false)})
            };

            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        [TestMethod]
        public void SelectNights_NavigationOptions_Valid()
        {
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.SelectDays,
                PageViewModelProvider = new MockViewProvider(new SelectNightsViewModel() { ValidationProvider = new MockValidator(true) })
            };

            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        //[TestMethod]
        //public void OpenFiles_NavigationOptions()
        //{
        //    NavigationViewModel navigationViewModel = new() { CurrentStep = NavigationStep.OpenFiles };

        //    Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
        //    Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
        //    Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
        //    Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
        //    Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
        //    Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        //}

        [TestMethod]
        public void SelectSignals_NavigationOptions_Invalid()
        {
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.SelectSignals,
                PageViewModelProvider = new MockViewProvider(new SelectSignalsViewModel() { ValidationProvider = new MockValidator(false) })
            };

            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        [TestMethod]
        public void SelectSignals_NavigationOptions_Valid()
        {
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.SelectSignals,
                PageViewModelProvider = new MockViewProvider(new SelectSignalsViewModel() { ValidationProvider = new MockValidator(true) })
            };

            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        [TestMethod]
        public void Settings_NavigationOptions_Invalid()
        {
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.Settings,
                PageViewModelProvider = new MockViewProvider(new ExportOptionsViewModel() { ValidationProvider = new MockValidator(false) })
            };

            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        [TestMethod]
        public void Settings_NavigationOptions_Valid()
        {
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.Settings,
                PageViewModelProvider = new MockViewProvider(new ExportOptionsViewModel() { ValidationProvider = new MockValidator(true) })
            };

            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        [TestMethod]
        public void Export_NavigationOptions()
        {
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.Export,
                PageViewModelProvider = new MockViewProvider(new SavedFilesViewModel())
            };

            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        #endregion

        #region Back and Forth (AchievedStep vs availability)


        #endregion

        #endregion
    }
}
