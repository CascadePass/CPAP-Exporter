namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class InverseBooleanConverterTests
    {
        [TestMethod]
        public void Convert_ShouldReturnTrue_ForFalseInput()
        {
            bool input = false;

            InverseBooleanConverter converter = new();
            var result = (bool)converter.Convert(input, null, null, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Convert_ShouldReturnFalse_ForTrueInput()
        {
            bool input = true;

            InverseBooleanConverter converter = new();
            var result = (bool)converter.Convert(input, null, null, null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Convert_NotBoolean_DefaultsToFalse()
        {
            string input = Guid.NewGuid().ToString();

            InverseBooleanConverter converter = new();
            var result = (bool)converter.Convert(input, null, null, null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ConvertBack_True_ReturnsFalse()
        {
            bool input = true;

            InverseBooleanConverter converter = new();
            var result = (bool)converter.ConvertBack(input, null, null, null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ConvertBack_False_ReturnsTrue()
        {
            bool input = false;

            InverseBooleanConverter converter = new();
            var result = (bool)converter.ConvertBack(input, null, null, null);

            Assert.IsTrue(result);
        }
    }
}
