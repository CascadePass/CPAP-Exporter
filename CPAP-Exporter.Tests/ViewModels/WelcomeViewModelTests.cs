namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class WelcomeViewModelTests
    {
        #region Title/Description Validation

        [TestMethod]
        public void WelcomeViewModel_HasCorrectTitle()
        {
            var vm = new WelcomeViewModel();
            Assert.AreEqual(Resources.PageTitle_Welcome, vm.Title);
        }

        [TestMethod]
        public void WelcomeViewModel_HasCorrectDescription()
        {
            var vm = new WelcomeViewModel();
            Assert.AreEqual(string.Empty, vm.PageDescription);
        }

        #endregion
    }
}
