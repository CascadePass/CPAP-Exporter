using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Interaction logic for PageView.xaml
    /// </summary>
    public partial class PageView : UserControl
    {
        public PageView()
        {
            this.InitializeComponent();

            this.DataContextChanged += this.PageView_DataContextChanged;
        }

        private void PageView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext is NavigationViewModel navViewModel)
            {
                navViewModel.PropertyChanged += NavigationViewModel_PropertyChanged;
            }
        }

        private void NavigationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (e.PropertyName == nameof(NavigationViewModel.CurrentView))
                {
                    this.UpdateBindings(this);
                }

                if (e.PropertyName == nameof(NavigationViewModel.CurrentView))
                {
                    if (this.DataContext is NavigationViewModel navViewModel && navViewModel.CurrentView.DataContext is PageViewModel pageViewModel)
                    {
                        // TODO: Unsubscribe from the previous PageViewModel's PropertyChanged event

                        pageViewModel.PropertyChanged += this.PageViewModel_PropertyChanged;
                    }
                }
            });
        }

        private void UpdateBindings(DependencyObject obj)
        {
            if (obj == null)
            {
                return;
            }

            if (obj.Dispatcher.CheckAccess())
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    this.UpdateBindings(child);
                }

                var localValues = obj.GetLocalValueEnumerator();
                while (localValues.MoveNext())
                {
                    var entry = localValues.Current;
                    if (entry.Property != null && BindingOperations.IsDataBound(obj, entry.Property))
                    {
                        var binding = BindingOperations.GetBindingExpression(obj, entry.Property);
                        binding?.UpdateTarget();
                    }
                }
            }
            else
            {
                // Invoke the dispatcher to run the logic on the UI thread
                obj.Dispatcher.Invoke(() => this.UpdateBindings(obj));
            }
        }

        private void UpdateProgressBarVisibility()
        {
            if (this.Dispatcher.CheckAccess())
            {

            }
            else
            {
                // Invoke the dispatcher to run the logic on the UI thread
                this.Dispatcher.Invoke(() => this.UpdateProgressBarVisibility());
            }
        }

        private void PageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PageViewModel.StatusContent))
            {
                if (this.Dispatcher.CheckAccess())
                {
                    this.UpdateProgressBarVisibility();
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        this.UpdateProgressBarVisibility();
                    });
                }
            }
        }
    }
}
