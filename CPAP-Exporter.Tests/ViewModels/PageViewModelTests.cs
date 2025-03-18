namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class PageViewModelTests
    {
        [TestMethod]
        public void PageViewModel_Constructor_SetsTitle()
        {
            string expectedTitle = "Test Title";
            string expectedDescription = "Test Description";

            PageViewModel viewModel = new(expectedTitle, expectedDescription);

            Assert.AreEqual(expectedTitle, viewModel.Title);
        }

        [TestMethod]
        public void PageViewModel_Constructor_SetsDescription()
        {
            string expectedTitle = "Test Title";
            string expectedDescription = "Test Description";
            PageViewModel viewModel = new(expectedTitle, expectedDescription);
            Assert.AreEqual(expectedDescription, viewModel.PageDescription);
        }

    }
}
