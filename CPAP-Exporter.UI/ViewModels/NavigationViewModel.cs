using CascadePass.CPAPExporter.Core;
using System.IO;
using System.Windows;

namespace CascadePass.CPAPExporter
{
    public class NavigationViewModel : ViewModel
    {
        #region Private fields

        private bool showNavButtonLabels;
        private FrameworkElement currentView;
        private NavigationStep currentPage;
        private ExportParameters exportParameters;
        private DelegateCommand openFilesCommand, selectNightsCommand, selectSignalsCommand, showExportSettingsCommand, exportCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="NavigationViewModel"/> class.
        /// </summary>
        public NavigationViewModel()
        {
            this.ExportParameters = new();
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
                if (this.SetPropertyValue(ref this.currentView, value, nameof(this.CurrentView)))
                {
                    if (old?.DataContext is ViewModel oldViewModel)
                    {
                        this.Unsubscribe(oldViewModel);
                    }

                    if (value?.DataContext is ViewModel newViewModel)
                    {
                        this.Subscribe(value.DataContext as ViewModel);
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
                        nameof(this.SettingsButtonImageUri),
                        nameof(this.ExportButtonImageUri),
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

        public bool ShowNavigationButtonLabels
        {
            get => this.showNavButtonLabels;
            set => this.SetPropertyValue(ref this.showNavButtonLabels, value, nameof(this.ShowNavigationButtonLabels));
        }

        #region Button Styles

        public Style OpenButtonStyle => this.GetButtonStyle(NavigationStep.OpenFiles);

        public Style SelectNightButtonStyle => this.GetButtonStyle(NavigationStep.SelectDays);

        public Style SelectSignalsButtonStyle => this.GetButtonStyle(NavigationStep.SelectSignals);

        public Style SettingsButtonStyle => this.GetButtonStyle(NavigationStep.Settings);

        public Style ExportButtonStyle => this.GetButtonStyle(NavigationStep.Export);

        #endregion

        #region Button Icons

        public string OpenButtonImageUri => this.currentPage == NavigationStep.OpenFiles ? "/Images/Navigation.OpenFiles.Selected.png" : "/Images/Navigation.OpenFiles.Normal.png";

        public string SelectNightButtonImageUri => this.currentPage == NavigationStep.SelectDays ? "/Images/Navigation.SelectNights.Selected.png" : "/Images/Navigation.SelectNights.Normal.png";

        public string SelectSignalsButtonImageUri => this.currentPage == NavigationStep.SelectSignals ? "/Images/Navigation.SelectSignals.Selected.png" : "/Images/Navigation.SelectSignals.Normal.png";

        public string SettingsButtonImageUri => this.currentPage == NavigationStep.Settings ? "/Images/Navigation.ExportSettings.Selected.png" : "/Images/Navigation.ExportSettings.Normal.png";

        public string ExportButtonImageUri => this.currentPage == NavigationStep.Export ? "/Images/Navigation.Save.Selected.png" : "/Images/Navigation.Save.Normal.png";

        #endregion

        #region Button clicks

        public DelegateCommand OpenFilesCommand => this.openFilesCommand ??= new DelegateCommand(this.OpenFiles);

        public DelegateCommand SelectNightsCommand => this.selectNightsCommand ??= new DelegateCommand(this.SelectNights);

        public DelegateCommand SelectSignalsCommand => this.selectSignalsCommand ??= new DelegateCommand(this.ShowSignals);

        public DelegateCommand ShowExportSettingsCommand => this.showExportSettingsCommand ??= new DelegateCommand(this.ShowExportSettings);

        public DelegateCommand ExportCommand => this.exportCommand ??= new DelegateCommand(this.Export);

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
