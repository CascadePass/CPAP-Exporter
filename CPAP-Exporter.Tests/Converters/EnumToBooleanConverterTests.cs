using CascadePass.CPAPExporter.Core;
using System.Globalization;
using System.Windows.Data;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class EnumToBooleanConverterTests
    {
        [TestMethod]
        public void Convert_ShouldReturnTrue_WhenEnumValueMatchesParameter()
        {
            var converter = new EnumToBooleanConverter();
            var value = OutputFileRule.OneFilePerNight;
            var parameter = "OneFilePerNight";

            var result = converter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);

            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void Convert_ShouldReturnFalse_WhenEnumValueDoesNotMatchParameter()
        {
            var converter = new EnumToBooleanConverter();
            var value = OutputFileRule.CombinedIntoSingleFile;
            var parameter = "OneFilePerNight";

            var result = converter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);

            Assert.IsFalse((bool)result);
        }

        [TestMethod]
        public void ConvertBack_ShouldReturnEnum_WhenParameterIsValidAndValueIsTrue()
        {
            var converter = new EnumToBooleanConverter();
            var value = true;
            var parameter = "CombinedIntoSingleFile";

            var result = converter.ConvertBack(value, typeof(OutputFileRule), parameter, CultureInfo.InvariantCulture);

            Assert.AreEqual(OutputFileRule.CombinedIntoSingleFile, result);
        }

        [TestMethod]
        public void ConvertBack_ShouldReturnBindingDoNothing_WhenValueIsFalse()
        {
            var converter = new EnumToBooleanConverter();
            var value = false;
            var parameter = "OneFilePerNight";

            var result = converter.ConvertBack(value, typeof(OutputFileRule), parameter, CultureInfo.InvariantCulture);

            Assert.AreEqual(Binding.DoNothing, result);
        }
    }
}