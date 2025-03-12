using CascadePass.CPAPExporter.Core;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class SignalViewModelTests
    {
        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            var signalInfo = new SignalInfo();
            var viewModel = new SignalViewModel(signalInfo);

            Assert.IsTrue(viewModel.IsSelected);
            Assert.AreEqual(signalInfo, viewModel.SignalInfo);
        }

        [TestMethod]
        public void IsSelected_SetterUpdatesProperty()
        {
            var viewModel = new SignalViewModel(new SignalInfo());
            viewModel.IsSelected = false;

            Assert.IsFalse(viewModel.IsSelected);
        }

        [TestMethod]
        public void SignalInfo_SetterUpdatesProperty()
        {
            var initialSignalInfo = new SignalInfo();
            var newSignalInfo = new SignalInfo();
            var viewModel = new SignalViewModel(initialSignalInfo);

            viewModel.SignalInfo = newSignalInfo;

            Assert.AreEqual(newSignalInfo, viewModel.SignalInfo);
        }
    }
}
