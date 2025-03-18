using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public partial class NavigationBar : UserControl
    {
        public NavigationBar()
        {
            this.InitializeComponent();

            if (this.DataContext is Observable observable)
            {
                observable.PropertyChanged += this.Observable_PropertyChanged;
            }
        }

        private void Observable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NavigationBar.UpdateButtonStyles(this);
        }

        private static void UpdateButtonStyles(DependencyObject parent)
        {
            if (parent.Dispatcher.CheckAccess())
            {
                // The current thread has access, proceed with the logic
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);

                    if (child is Button button)
                    {
                        var binding = BindingOperations.GetBindingExpression(button, Button.StyleProperty);
                        binding?.UpdateTarget();
                    }
                    else
                    {
                        UpdateButtonStyles(child);
                    }
                }
            }
            else
            {
                // Invoke the dispatcher to run the logic on the UI thread
                parent.Dispatcher.Invoke(() => UpdateButtonStyles(parent));
            }
        }
    }
}
