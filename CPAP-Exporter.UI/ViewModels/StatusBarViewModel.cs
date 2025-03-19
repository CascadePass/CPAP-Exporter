using System.Diagnostics;

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

            this.pageViewModelProvider = ApplicationComponentProvider.PageViewModelProvider;

            if (ApplicationComponentProvider.Status is Observable observable)
            {
                observable.PropertyChanged += Observable_PropertyChanged;
            }
        }

        public StatusBarViewModel(MainWindow mainWindow, NavigationViewModel navigationViewModel) : this()
        {
            this.MainWindow = mainWindow;
            this.NavigationViewModel = navigationViewModel;
        }

        #endregion

        public MainWindow MainWindow
        {
            get => this.mainWindow;
            set => this.SetPropertyValue(ref this.mainWindow, value, nameof(this.MainWindow));
        }

        public NavigationViewModel NavigationViewModel { get; set; }

        public PageViewModel CurrentViewModel => this.MainWindow?.PageViewer.DataContext is PageViewModel ? this.PageViewModelProvider.GetViewModel(this.MainWindow?.PageViewer) : null;

        public string StatusText => ApplicationComponentProvider.Status.StatusText;

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
                    throw new InvalidOperationException(Resources.Validation_MainWindow_Null);
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

        private void Observable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.StatusText));
        }
    }
}
