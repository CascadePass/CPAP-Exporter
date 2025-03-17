using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter
{
    public class StatusBarViewModel : ViewModel
    {
        #region Fields

        private MainWindow mainWindow;
        private Version version;
        private IPageViewModelProvider pageViewModelProvider;
        private DelegateCommand viewReleasesPageCommand;

        #endregion

        #region Constructors

        public StatusBarViewModel()
        {
            this.Version = this.GetType().Assembly.GetName().Version;

            this.pageViewModelProvider = new PageViewModelProvider();
        }

        public StatusBarViewModel(MainWindow mainWindow, NavigationViewModel navigationViewModel) : this()
        {
            this.MainWindow = mainWindow;
            this.NavigationViewModel = navigationViewModel;

            this.NavigationViewModel.PropertyChanged += NavigationViewModel_PropertyChanged;
        }

        #endregion

        public MainWindow MainWindow
        {
            get => this.mainWindow;
            set => this.SetPropertyValue(ref this.mainWindow, value, nameof(this.MainWindow));
        }

        public NavigationViewModel NavigationViewModel { get; set; }

        public PageViewModel CurrentViewModel => this.MainWindow?.PageViewer.DataContext is PageViewModel ? this.PageViewModelProvider.GetViewModel(this.MainWindow?.PageViewer) : null;

        public string StatusText => this.CurrentViewModel?.StatusText;

        public double FontSize
        {
            get => this.MainWindow?.FontSize ?? default;
            set
            {
                if (this.MainWindow != null)
                {
                    this.MainWindow.FontSize = value;
                }
                else
                {
                    throw new InvalidOperationException("MainWindow is not set.");
                }
            }
        }

        public Version Version
        {
            get => this.version;
            set => this.SetPropertyValue(ref this.version, value, nameof(this.Version));
        }

        public IPageViewModelProvider PageViewModelProvider
        {
            get => this.pageViewModelProvider;
#if DEBUG
            set => this.SetPropertyValue(ref this.pageViewModelProvider, value, nameof(this.PageViewModelProvider));
#endif
        }

        public DelegateCommand ViewReleasesPageCommand => this.viewReleasesPageCommand ??= new DelegateCommand(this.ViewReleasesPage);

        private void ViewReleasesPage()
        {
            try
            {
                Process.Start("https://github.com/CascadePass/CPAP-Exporter/releases");
            }
            catch (Exception)
            {
            }
        }

        private void NavigationViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(NavigationViewModel.CurrentView))
            {
                this.OnPropertyChanged(nameof(this.CurrentViewModel));
                this.OnPropertyChanged(nameof(this.StatusText));
            }
        }
    }
}
