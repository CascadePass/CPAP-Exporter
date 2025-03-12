using System.Windows;

namespace CascadePass.CPAPExporter
{
    public class PageViewModelProvider : IPageViewModelProvider
    {
        public PageViewModel GetViewModel(FrameworkElement frameworkElement)
        {
            if (frameworkElement is null)
            {
                return null;
            }

            var viewModel = frameworkElement.DataContext as PageViewModel ?? throw new ArgumentException(null, nameof(frameworkElement));

            return viewModel;
        }
    }
}
