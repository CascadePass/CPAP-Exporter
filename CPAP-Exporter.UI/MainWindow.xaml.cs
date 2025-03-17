using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var navigationViewModel = this.NavBar.DataContext as NavigationViewModel;
            
            this.PageViewer.DataContext = navigationViewModel;
            this.StatusStrip.DataContext = new StatusBarViewModel() { MainWindow = this, NavigationViewModel = navigationViewModel };
            
            navigationViewModel.PropertyChanged += NavigationViewModel_PropertyChanged;
        }

        private void NavigationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (string.Equals(e.PropertyName, nameof(NavigationViewModel.CurrentView)))
            {
                var statusStripBinding = BindingOperations.GetBindingExpression(this.StatusStrip, DataContextProperty);
                statusStripBinding?.UpdateTarget();
            }

            //if (string.Equals(e.PropertyName, nameof(PageViewModel.IsBusy)))
            //{
            //    var bannerStripBinding = BindingOperations.GetBindingExpression(this.BannerStrip, VisibilityProperty);
            //    bannerStripBinding?.UpdateTarget();
            //}
        }
    }
}