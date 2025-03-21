using Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.ObjectModel;
using System.ComponentModel;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class NavigationViewModelTests
    {
        private const string CURRENT_STYLE = "SelectedNavigationButtonStyle";
        private const string DISABLED_STYLE = "DisabledButtonStyle";
        private const string NORMAL_STYLE = "NavigationButtonStyle";

        [TestMethod]
        public void NewInstance_CurrentPageIsWelcomeScreen()
        {
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new WelcomeViewModel());
            NavigationViewModel navigationViewModel = new();

            Assert.AreEqual(NavigationStep.Welcome, navigationViewModel.CurrentStep);
        }

        [TestMethod]
        public void NewInstance_HasExportParameters()
        {
            NavigationViewModel navigationViewModel = new();

            Assert.IsNotNull(navigationViewModel.ExportParameters);
        }

        [TestMethod]
        public void Subscribe_AttachesEventHandler()
        {
            var viewModel = new MockPageViewModel();
            bool eventHandled = false;

            void Handler(object sender, PropertyChangedEventArgs args)
            {
                eventHandled = true;
            }

            var testClass = new NavigationViewModel();
            viewModel.PropertyChanged += Handler;

            testClass.Subscribe(viewModel);
            viewModel.Value = Guid.NewGuid().ToString();

            Assert.IsTrue(eventHandled);
        }

        [TestMethod]
        public void Unsubscribe_RemovesEventHandler()
        {
            var viewModel = new MockPageViewModel();
            bool eventHandled = false;

            void Handler(object sender, PropertyChangedEventArgs args)
            {
                eventHandled = true;
            }

            var testClass = new NavigationViewModel();
            testClass.Subscribe(viewModel);

            testClass.Unsubscribe(viewModel);
            viewModel.Value = Guid.NewGuid().ToString();

            Assert.IsFalse(eventHandled);
        }


        #region Button Styles

        #region Happy Path (CurrentStep vs availability)

        [TestMethod]
        public void Welcome_NavigationOptions()
        {
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new WelcomeViewModel());
            NavigationViewModel navigationViewModel = new() {
                CurrentStep = NavigationStep.Welcome,
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
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new SelectNightsViewModel() { ValidationProvider = new MockValidator(false) });
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.SelectDays,
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
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new SelectNightsViewModel() { ValidationProvider = new MockValidator(true) });
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.SelectDays,
            };

            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        [TestMethod]
        public void OpenFiles_NavigationOptions_Valid()
        {
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new WelcomeViewModel() { ValidationProvider = new MockValidator(true) });
            NavigationViewModel navigationViewModel = new() { CurrentStep = NavigationStep.OpenFiles };

            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        [TestMethod]
        public void OpenFiles_NavigationOptions_Invalid()
        {
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new WelcomeViewModel() { ValidationProvider = new MockValidator(false) });
            NavigationViewModel navigationViewModel = new() { CurrentStep = NavigationStep.OpenFiles };

            Assert.AreEqual(NORMAL_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Welcome));
            Assert.AreEqual(CURRENT_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.OpenFiles));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectDays));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.SelectSignals));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Settings));
            Assert.AreEqual(DISABLED_STYLE, navigationViewModel.GetButtonStyleName(NavigationStep.Export));
        }

        [TestMethod]
        public void SelectSignals_NavigationOptions_Invalid()
        {
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new SelectSignalsViewModel() { ValidationProvider = new MockValidator(false) });
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.SelectSignals,
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
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new SelectSignalsViewModel() { ValidationProvider = new MockValidator(true) });
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.SelectSignals,
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
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new ExportOptionsPageViewModel(new()) { ValidationProvider = new MockValidator(false) });
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.Settings,
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
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new ExportOptionsPageViewModel(new()) { ValidationProvider = new MockValidator(true) });
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.Settings,
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
            ApplicationComponentProvider.PageViewModelProvider = new MockViewProvider(new SavedFilesViewModel() { ValidationProvider = new MockValidator(true) });
            NavigationViewModel navigationViewModel = new()
            {
                CurrentStep = NavigationStep.Export,
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
