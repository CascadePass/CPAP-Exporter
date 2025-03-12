using System.Windows;

namespace CascadePass.CPAPExporter
{
    public interface IPageViewModelProvider
    {
        PageViewModel GetViewModel(FrameworkElement frameworkElement);
    }
}
