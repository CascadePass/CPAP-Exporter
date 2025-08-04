using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CascadePass.CPAPExporter
{
    public class NavigationViewModel : ViewModel
    {
        #region Private fields

        private bool showNavButtonLabels;
        private FrameworkElement currentView;
        private NavigationStep currentPage;
        private ExportParameters exportParameters;
        private DelegateCommand
            openFilesCommand, selectNightsCommand, selectSignalsCommand, showExportSettingsCommand, exportCommand, viewReleaseNotes,
            toggleNavigationDrawerExpansionCommand, settingsCommand
        ;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="NavigationViewModel"/> class.
        /// </summary>
        public NavigationViewModel()
        {
            this.ExportParameters = new();

            // Use configured value to start
            this.ShowNavigationButtonLabels = this.ExportParameters.UserPreferences.IsNavigationDrawerExpanded;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="CurrentView"/>, a <see cref="FrameworkElement"/>
        /// which is shown to the user as the main application content.
        /// </summary>
        public FrameworkElement CurrentView
        {
            get
            {
                if (this.currentView == null)
                {
                    this.OpenFiles();
                }

                return this.currentView;
            }
            set
            {
                var old = this.currentView;
                if (this.SetPropertyValue(ref this.currentView, value, [nameof(this.CurrentView), nameof(this.SettingsIcon)]))
                {
                    if (old?.DataContext is ViewModel oldViewModel)
                    {
                        this.Unsubscribe(oldViewModel);
                    }

                    if (value?.DataContext is ViewModel newViewModel)
                    {
                        this.Subscribe(value.DataContext as ViewModel);
                    }

                    if (value?.DataContext is PageViewModel newPage)
                    {
                        newPage.BecomeVisible();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the current step in the navigation process.
        /// </summary>
        public NavigationStep CurrentStep
        {
            get => this.currentPage;
            set
            {
                if (value > this.AchievedStep)
                {
                    this.AchievedStep = value;
                }

                bool changed = this.SetPropertyValue(
                    ref this.currentPage,
                    value,
                    [
                        nameof(this.CurrentStep),
                        nameof(this.OpenButtonStyle),
                        nameof(this.SelectNightButtonStyle),
                        nameof(this.SelectSignalsButtonStyle),
                        nameof(this.SettingsButtonStyle),
                        nameof(this.ExportButtonStyle),
                        nameof(this.OpenButtonImageUri),
                        nameof(this.SelectNightButtonImageUri),
                        nameof(this.SelectSignalsButtonImageUri),
                        nameof(this.ExportSettingsButtonImageUri),
                        nameof(this.ExportButtonImageUri),
                        nameof(this.SettingsIcon),
                    ]
                );
            }
        }

        /// <summary>
        /// Gets or sets the highest step that the user has achieved.
        /// </summary>
        public NavigationStep AchievedStep { get; internal set; }

        /// <summary>
        /// Gets or sets required details for the export process.
        /// </summary>
        public ExportParameters ExportParameters
        {
            get => this.exportParameters;
            set => this.SetPropertyValue(ref this.exportParameters, value, nameof(this.ExportParameters));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the navigation tray is open.
        /// </summary>
        public bool ShowNavigationButtonLabels
        {
            get => this.showNavButtonLabels;
            set
            {
                var changed = this.SetPropertyValue(ref this.showNavButtonLabels, value, [
                    nameof(this.ShowNavigationButtonLabels),
                    nameof(this.MenuIcon),
                    nameof(this.NavigationTrayWidth),
                    nameof(this.SettingsIcon),
                    ]
                );

                if (changed)
                {
                    this.ExportParameters.UserPreferences.IsNavigationDrawerExpanded = value;
                }
            }
        }

        public double NavigationTrayWidth => this.ShowNavigationButtonLabels ? 160 : 64;

        #region Button Styles

        public Style OpenButtonStyle => this.GetButtonStyle(NavigationStep.OpenFiles);

        public Style SelectNightButtonStyle => this.GetButtonStyle(NavigationStep.SelectDays);

        public Style SelectSignalsButtonStyle => this.GetButtonStyle(NavigationStep.SelectSignals);

        public Style SettingsButtonStyle => this.GetButtonStyle(NavigationStep.Settings);

        public Style ExportButtonStyle => this.GetButtonStyle(NavigationStep.Export);

        #endregion

        #region Button Icons

        public string OpenButtonImageUri => this.IsStepAllowed(NavigationStep.OpenFiles) ? "/Images/Navigation.OpenFiles.Selected.png" : "/Images/Navigation.OpenFiles.Normal.png";

        public string SelectNightButtonImageUri => this.IsStepAllowed(NavigationStep.SelectDays) ? "/Images/Navigation.SelectNights.Selected.png" : "/Images/Navigation.SelectNights.Normal.png";

        public string SelectSignalsButtonImageUri => this.IsStepAllowed(NavigationStep.SelectSignals) ? "/Images/Navigation.SelectSignals.Selected.png" : "/Images/Navigation.SelectSignals.Normal.png";

        public string ExportSettingsButtonImageUri => this.IsStepAllowed(NavigationStep.Settings) ? "/Images/Navigation.ExportSettings.Selected.png" : "/Images/Navigation.ExportSettings.Normal.png";

        public string ExportButtonImageUri => this.IsStepAllowed(NavigationStep.Export) ? "/Images/Navigation.Save.Selected.png" : "/Images/Navigation.Save.Normal.png";

        public BitmapImage MenuIcon => this.ShowNavigationButtonLabels ? Application.Current?.FindResource("Menu.Icon.Selected") as BitmapImage : Application.Current?.FindResource("Menu.Icon") as BitmapImage;

        public BitmapImage SettingsIcon => this.CurrentView is SettingsView ? Application.Current?.FindResource("Settings.Icon.Selected") as BitmapImage : Application.Current?.FindResource("Settings.Icon") as BitmapImage;

        #endregion

        #region Button clicks

        public DelegateCommand OpenFilesCommand => this.openFilesCommand ??= new DelegateCommand(this.OpenFiles);

        public DelegateCommand SelectNightsCommand => this.selectNightsCommand ??= new DelegateCommand(this.SelectNights);

        public DelegateCommand SelectSignalsCommand => this.selectSignalsCommand ??= new DelegateCommand(this.ShowSignals);

        public DelegateCommand ShowExportSettingsCommand => this.showExportSettingsCommand ??= new DelegateCommand(this.ShowExportSettings);

        public DelegateCommand ExportCommand => this.exportCommand ??= new DelegateCommand(this.Export);

        public DelegateCommand ShowReleaseNotesCommand => this.viewReleaseNotes ??= new DelegateCommand(this.ShowReleaseNotes);

        public DelegateCommand ToggleNavDrawerExpansionCommand => this.toggleNavigationDrawerExpansionCommand ??= new DelegateCommand(this.ToggleNavigationDrawerExpansion);

        public DelegateCommand ShowSettingsCommand => this.settingsCommand ??= new DelegateCommand(this.ShowSettings);

        #endregion

        #endregion

        #region Methods

        public void OpenFiles()
        {
            var viewModel = new OpenFilesViewModel(this.exportParameters);
            this.CurrentView = new OpenFilesView() { DataContext = viewModel };
            this.CurrentStep = NavigationStep.OpenFiles;
        }

        public void SelectNights()
        {
            this.CurrentStep = NavigationStep.SelectDays;

            if (this.CurrentView is not SelectNightsView selectNightsView)
            {
                var viewModel = new SelectNightsViewModel(this.exportParameters);
                var view = new SelectNightsView { DataContext = viewModel };

                this.CurrentView = view;
            }
            else
            {
                var vm = (SelectNightsViewModel)this.CurrentView.DataContext;

                vm.ShowDefaultStatusMessage();
            }
        }

        public void ShowSignals()
        {
            var viewModel = new SelectSignalsViewModel(this.ExportParameters);

            this.CurrentView = new SelectSignalsView { DataContext = viewModel };
            this.CurrentStep = NavigationStep.SelectSignals;

            ApplicationComponentProvider.Status.StatusText = string.Format(Resources.SignalsAvailable, viewModel.Signals.Count);
        }

        public void ShowExportSettings()
        {
            this.CurrentView = new OptionsView() { DataContext = new ExportOptionsPageViewModel(this.ExportParameters) };
            this.CurrentStep = NavigationStep.Settings;

            ApplicationComponentProvider.Status.StatusText = Resources.ReadyToExport;
        }

        public void Export()
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog();
            string folder = null;

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            folder = dialog.FolderName;

            SavedFilesView savedView = new();
            var savedVM = (SavedFilesViewModel)savedView.DataContext;
            savedVM.ExportParameters = this.ExportParameters;

            this.CurrentStep = NavigationStep.Export;
            this.CurrentView = savedView;

            savedVM.PerformExportAsync(folder);

            //ApplicationComponentProvider.Status.StatusText = string.Format(Resources.FilesWritten, savedVM.Files.Count);
        }

        public void ShowHashView()
        {
            var curView = this.CurrentView;

            var hashView = new HashesView() { DataContext = new HashesViewModel() };
            var hashesViewModel = (HashesViewModel)hashView.DataContext;

            Window mainWindow = Application.Current.MainWindow;
            Window host = new() {
                Owner = mainWindow,
                Content = hashView,
                Top = mainWindow.Top,
                Left = mainWindow.Left,
                Width = mainWindow.Width,
                Height = mainWindow.Height,
            };

            host.ShowDialog();
        }

        public void ShowReleaseNotes()
        {
            if (!Uri.IsWellFormedUriString(Resources.ReleaseNotesUri, UriKind.Absolute))
            {
                Debug.WriteLine("Invalid ReleaseNotesUri. Make sure it's set correctly in Resources.");
                Debugger.Break();
                return;
            }

            try
            {
                var uri = Resources.ReleaseNotesUri;

                var psi = new ProcessStartInfo
                {
                    FileName = uri,
                    UseShellExecute = true
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening release notes: {ex.Message}");
                Debugger.Break();
            }
        }

        public void ShowSettings()
        {
            if (this.CurrentView is not SettingsView settingsView)
            {
                settingsView = new SettingsView();
                var viewModel = (SettingsViewModel)settingsView.DataContext;

                viewModel.ExportParameters = this.ExportParameters;
            }

            this.CurrentView = settingsView;
        }

        public void ToggleNavigationDrawerExpansion()
        {
            this.ShowNavigationButtonLabels = !this.ShowNavigationButtonLabels;
        }

        #region Helper methods

        private Style GetButtonStyle(NavigationStep step) => (Style)Application.Current.FindResource(this.GetButtonStyleName(step));

        public string GetButtonStyleName(NavigationStep step)
        {
            if (this.currentPage == step)
            {
                return "SelectedNavigationButtonStyle";
            }

            int validationOffset = ApplicationComponentProvider.PageViewModelProvider.GetViewModel(this.currentView)?.IsValid ?? false ? 1 : 0;
            if (this.AchievedStep + validationOffset < step)
            {
                return "DisabledButtonStyle";
            }

            return "NavigationButtonStyle";
        }

        public bool IsStepAllowed(NavigationStep step)
        {
            if (this.currentPage == step)
            {
                return true;
            }

            int validationOffset = ApplicationComponentProvider.PageViewModelProvider.GetViewModel(this.currentView)?.IsValid ?? false ? 1 : 0;
            if (this.AchievedStep + validationOffset < step)
            {
                return false;
            }

            return true;
        }

        internal void UpdateButtonImages()
        {
            this.OnPropertyChanged(nameof(this.OpenButtonImageUri));
            this.OnPropertyChanged(nameof(this.SelectNightButtonImageUri));
            this.OnPropertyChanged(nameof(this.SelectSignalsButtonImageUri));
            this.OnPropertyChanged(nameof(this.ExportSettingsButtonImageUri));
            this.OnPropertyChanged(nameof(this.ExportButtonImageUri));
            this.OnPropertyChanged(nameof(this.MenuIcon));
            this.OnPropertyChanged(nameof(this.SettingsIcon));
        }

        public void Subscribe(ViewModel viewModel)
        {
            viewModel.PropertyChanged += this.ViewModel_PropertyChanged;

            if (viewModel is PageViewModel pageViewModel)
            {
                pageViewModel.AdvancePage += this.PageViewModel_AdvancePage;
            }
        }

        public void Unsubscribe(ViewModel viewModel)
        {
            viewModel.PropertyChanged -= this.ViewModel_PropertyChanged;

            if (viewModel is PageViewModel pageViewModel)
            {
                pageViewModel.AdvancePage -= this.PageViewModel_AdvancePage;
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.CurrentView));
        }

        private void PageViewModel_AdvancePage(object sender, EventArgs e)
        {
            if (sender is OpenFilesViewModel)
            {
                this.SelectNights();
            }
        }

        #endregion

        #endregion
    }
}
