using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class StringToVisibilityConverterTests
    {
        [TestMethod]
        public void Convert_Null()
        {
            StringToVisibilityConverter converter = new();

            var result = converter.Convert(null, null, null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void Convert_EmptyString()
        {
            StringToVisibilityConverter converter = new();

            var result = converter.Convert(string.Empty, null, null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void Convert_Space()
        {
            StringToVisibilityConverter converter = new();

            var result = converter.Convert(" ", null, null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void Convert_Tab()
        {
            StringToVisibilityConverter converter = new();

            var result = converter.Convert("\t", null, null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }


        [TestMethod]
        public void Convert_NewLine()
        {
            StringToVisibilityConverter converter = new();

            var result = converter.Convert(Environment.NewLine, null, null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void Convert_Text()
        {
            StringToVisibilityConverter converter = new();

            var result = converter.Convert(Guid.NewGuid().ToString(), null, null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void Convert_NotAString()
        {
            StringToVisibilityConverter converter = new();

            var result = converter.Convert(Guid.NewGuid(), null, null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }
    }
}
