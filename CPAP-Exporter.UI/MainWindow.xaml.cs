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
        private UserSettings userSettings;

        public MainWindow()
        {
            this.InitializeComponent();

            var navigationViewModel = this.NavBar.DataContext as NavigationViewModel;
            this.userSettings = navigationViewModel.ExportParameters.UserPreferences;

            this.PageViewer.DataContext = navigationViewModel;

            navigationViewModel.PropertyChanged += NavigationViewModel_PropertyChanged;

            this.SetWindowSizeAndLocation();
        }

        private void SetWindowSizeAndLocation()
        {
            if (this.userSettings.WindowX + this.userSettings.WindowY + this.userSettings.WindowWidth + this.userSettings.WindowHeight > 0)
            {
                this.Left = this.userSettings.WindowX;
                this.Top = this.userSettings.WindowY;
                this.Width = this.userSettings.WindowWidth;
                this.Height = this.userSettings.WindowHeight;
            }
        }

        private void GetWindowSizeAndLocation()
        {
            this.userSettings.WindowX = this.Left;
            this.userSettings.WindowY = this.Top;
            this.userSettings.WindowWidth = this.Width;
            this.userSettings.WindowHeight = this.Height;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this.GetWindowSizeAndLocation();
            this.userSettings.Save();
        }

        private void NavigationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (string.Equals(e.PropertyName, nameof(NavigationViewModel.CurrentView)))
            //{
            //}

            //if (string.Equals(e.PropertyName, nameof(PageViewModel.IsBusy)))
            //{
            //    var bannerStripBinding = BindingOperations.GetBindingExpression(this.BannerStrip, VisibilityProperty);
            //    bannerStripBinding?.UpdateTarget();
            //}
        }
    }
}