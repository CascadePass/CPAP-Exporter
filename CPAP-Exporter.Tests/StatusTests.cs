namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class StatusTests
    {
        private Status status;

        [TestInitialize]
        public void SetUp()
        {
            status = new Status();
        }

        [TestMethod]
        public void StatusText_SetAndGet_ReturnsCorrectValue()
        {
            string expected = "Test Status";

            status.StatusText = expected;

            Assert.AreEqual(expected, status.StatusText);
        }

        [TestMethod]
        public void StatusText_SetProperty_RaisesPropertyChangedEvent()
        {
            bool eventRaised = false;

            status.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Status.StatusText))
                {
                    eventRaised = true;
                }
            };

            status.StatusText = "New Status";

            Assert.IsTrue(eventRaised, "PropertyChanged event was not raised.");
        }
    }
}
