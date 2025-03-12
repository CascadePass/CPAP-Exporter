using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class ExportOptionsViewModelTests
    {
        #region Title/Description Validation

        [TestMethod]
        public void HasCorrectTitle()
        {
            var vm = new ExportOptionsViewModel();
            Assert.AreEqual(Resources.PageTitle_Options, vm.Title);
        }

        [TestMethod]
        public void HasCorrectDescription()
        {
            var vm = new ExportOptionsViewModel();
            Assert.AreEqual(Resources.PageDesc_Options, vm.PageDescription);
        }

        #endregion
    }
}
