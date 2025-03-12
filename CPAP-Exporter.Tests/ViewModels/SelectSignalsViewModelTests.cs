using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class SelectSignalsViewModelTests
    {
        #region Title/Description Validation

        [TestMethod]
        public void HasCorrectTitle()
        {
            var vm = new SelectSignalsViewModel();
            Assert.AreEqual(Resources.PageTitle_SelectSignals, vm.Title);
        }

        [TestMethod]
        public void HasCorrectDescription()
        {
            var vm = new SelectSignalsViewModel();
            Assert.AreEqual(Resources.PageDesc_SelectSignals, vm.PageDescription);
        }

        #endregion

    }
}
