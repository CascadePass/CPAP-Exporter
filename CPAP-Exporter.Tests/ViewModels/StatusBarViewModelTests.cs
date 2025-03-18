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

        [TestMethod]
        public void Version_IsValid()
        {
            object version = new StatusBarViewModel().Version;

            Console.WriteLine($"new StatusBarViewModel().Version == {version}");

            Assert.IsNotNull(version);
            Assert.IsInstanceOfType<Version>(version);
        }
    }
}
