using System.Windows;

namespace CascadePass.CPAPExporter.UI.Tests
{
    public class MockViewProvider : IPageViewModelProvider
    {
        public MockViewProvider(PageViewModel returnValue)
        {
            this.DesiredReturnValue = returnValue;
        }

        public PageViewModel DesiredReturnValue { get; set; }

        public PageViewModel GetViewModel(FrameworkElement frameworkElement)
        {
            return this.DesiredReturnValue;
        }
    }
}
