using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class StatusBarViewModelTests
    {
        [TestMethod]
        public void StatusBarViewModel_NoWindow()
        {
            StatusBarViewModel statusBarViewModel = new();

            Assert.IsNull(statusBarViewModel.MainWindow);
            Assert.IsNull(statusBarViewModel.StatusText);
        }

        [TestMethod]
        public void FontSize_Get_NoWindow()
        {
            StatusBarViewModel statusBarViewModel = new();

            Assert.AreEqual(default, statusBarViewModel.FontSize);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void FontSize_Set_NoWindow()
        {
            StatusBarViewModel statusBarViewModel = new();
            statusBarViewModel.FontSize = 12;
        }
    }
}
