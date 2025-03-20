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
        private IPageViewModelProvider pageViewModelProvider;
        private DelegateCommand showWelcomeScreenCommand, openFilesCommand, selectNightsCommand, selectSignalsCommand, showExportSettingsCommand, exportCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="NavigationViewModel"/> class.
        /// </summary>
        public NavigationViewModel()
        {
            this.pageViewModelProvider = new PageViewModelProvider();
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
                    this.ShowWelcomeScreen();
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
                        nameof(this.WelcomeButtonStyle),
                        nameof(this.OpenButtonStyle),
                        nameof(this.SelectNightButtonStyle),
                        nameof(this.SelectSignalsButtonStyle),
                        nameof(this.SettingsButtonStyle),
                        nameof(this.ExportButtonStyle)
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

        public IPageViewModelProvider PageViewModelProvider
        {
            get => this.pageViewModelProvider;
#if DEBUG
            set => this.SetPropertyValue(ref this.pageViewModelProvider, value, nameof(this.PageViewModelProvider));
#endif
        }

        #region Button Styles

        public Style WelcomeButtonStyle => this.GetButtonStyle(NavigationStep.Welcome);

        public Style OpenButtonStyle => this.GetButtonStyle(NavigationStep.OpenFiles);

        public Style SelectNightButtonStyle => this.GetButtonStyle(NavigationStep.SelectDays);

        public Style SelectSignalsButtonStyle => this.GetButtonStyle(NavigationStep.SelectSignals);

        public Style SettingsButtonStyle => this.GetButtonStyle(NavigationStep.Settings);

        public Style ExportButtonStyle => this.GetButtonStyle(NavigationStep.Export);

        #endregion

        #region Button clicks

        public DelegateCommand ShowWelcomeScreenCommand => this.showWelcomeScreenCommand ??= new DelegateCommand(this.ShowWelcomeScreen);

        public DelegateCommand OpenFilesCommand => this.openFilesCommand ??= new DelegateCommand(this.OpenFiles);

        public DelegateCommand SelectNightsCommand => this.selectNightsCommand ??= new DelegateCommand(this.SelectNights);

        public DelegateCommand SelectSignalsCommand => this.selectSignalsCommand ??= new DelegateCommand(this.ShowSignals);

        public DelegateCommand ShowExportSettingsCommand => this.showExportSettingsCommand ??= new DelegateCommand(this.ShowExportSettings);

        public DelegateCommand ExportCommand => this.exportCommand ??= new DelegateCommand(this.Export);

        #endregion

        #endregion

        #region Methods

        public void ShowWelcomeScreen()
        {
            this.CurrentView = new WelcomeView();
            this.CurrentStep = NavigationStep.Welcome;
        }

        public void OpenFiles()
        {
            var oldStep = this.currentPage;
            this.CurrentStep = NavigationStep.OpenFiles;

            var dialog = new Microsoft.Win32.OpenFolderDialog();

            if (dialog.ShowDialog() == true)
            {
                var folder = dialog.FolderName;

                if (!string.IsNullOrWhiteSpace(folder) && Directory.Exists(folder))
                {
                    this.exportParameters.SourcePath = folder;
                    this.exportParameters.Reports.Clear();
                    ApplicationComponentProvider.Status.StatusText = string.Format(Resources.ReadingFolder, folder);

                    var view = new SelectNightsView { DataContext = new SelectNightsViewModel(this.ExportParameters) };

                    this.CurrentView = view;
                    this.CurrentStep = NavigationStep.SelectDays;

                    // AchievedStep may be all the way to save, but need to reset to SelectDays
                    // to force the user to make selections against the newly loaded files.

                    this.AchievedStep = NavigationStep.SelectDays;
                }
            }
            else
            {
                this.currentPage = oldStep;
            }
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

            var vm = (SelectNightsViewModel)this.CurrentView.DataContext;

            ApplicationComponentProvider.Status.StatusText = string.Format(Resources.NightsAvailable, vm.Reports.Count, this.ExportParameters.SourcePath);
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



            CsvExportSettings csvSettings = (CsvExportSettings)this.ExportParameters.Settings.FirstOrDefault(s => s is CsvExportSettings);
            CsvExporter exporter = new(
                [.. this.exportParameters.Reports.Where(r => r.IsSelected).Select(r => r.DailyReport)],
                this.exportParameters.SignalNames
                );

            if (csvSettings.OutputFileHandling == OutputFileRule.CombinedIntoSingleFile)
            {
                exporter.ExportToFile(Path.Combine(folder, csvSettings.Filenames.First()));
                exporter.ExportFlaggedEventsToFile(Path.Combine(folder, csvSettings.EventFilenames.First()));

                savedVM.AddFile(Path.Combine(folder, csvSettings.Filenames.First()), Resources.FilesLabel_FullExport);
                savedVM.AddFile(Path.Combine(folder, csvSettings.EventFilenames.First()), Resources.FilesLabel_EventsExport);
            }
            else
            {
                exporter.DailyReports.Clear();
                for(int i = 0; i < this.exportParameters.Reports.Where(r => r.IsSelected).Count(); i++) {
                    exporter.DailyReports.Add(this.exportParameters.Reports.Where(r => r.IsSelected).ElementAt(i).DailyReport);

                    exporter.ExportToFile(Path.Combine(folder, csvSettings.Filenames[i]));
                    exporter.ExportFlaggedEventsToFile(Path.Combine(folder, csvSettings.EventFilenames[i]));

                    savedVM.AddFile(Path.Combine(folder, csvSettings.Filenames[i]), Resources.FilesLabel_FullExport);
                    savedVM.AddFile(Path.Combine(folder, csvSettings.EventFilenames[i]), Resources.FilesLabel_EventsExport);
                }
            }


            this.CurrentStep = NavigationStep.Export;
            this.CurrentView = savedView;

            ApplicationComponentProvider.Status.StatusText = string.Format(Resources.FilesWritten, savedVM.Files.Count);
        }

        #region Helper methods

        private Style GetButtonStyle(NavigationStep step) => (Style)Application.Current.FindResource(this.GetButtonStyleName(step));

        public string GetButtonStyleName(NavigationStep step)
        {
            if (this.currentPage == step)
            {
                return "SelectedNavigationButtonStyle";
            }

            int validationOffset = this.PageViewModelProvider.GetViewModel(this.currentView)?.IsValid ?? false ? 1 : 0;
            if (this.AchievedStep + validationOffset < step)
            {
                return "DisabledButtonStyle";
            }

            return "NavigationButtonStyle";
        }

        public void Subscribe(ViewModel viewModel)
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public void Unsubscribe(ViewModel viewModel)
        {
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.CurrentView));
        }

        #endregion

        #endregion
    }
}
