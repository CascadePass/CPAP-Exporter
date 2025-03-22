using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Interaction logic for StatusStrip.xaml
    /// </summary>
    public partial class StatusStrip : UserControl
    {
        public StatusStrip()
        {
            this.InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.DataContext is StatusBarViewModel statusBarViewModel)
            {
                statusBarViewModel.PropertyChanged += this.StatusBarViewModel_PropertyChanged;
            }
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

        private void StatusBarViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.UpdateBindings(this);
        }
    }
}
