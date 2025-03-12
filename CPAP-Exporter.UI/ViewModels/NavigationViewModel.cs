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
            this.PageViewModelProvider = new PageViewModelProvider();
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
                this.exportParameters.SourcePath = dialog.FolderName;

                if (!string.IsNullOrWhiteSpace(folder) && Directory.Exists(folder))
                {
                    this.exportParameters.SourcePath = folder;
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
        }

        public void ShowSignals()
        {
            var viewModel = new SelectSignalsViewModel(this.ExportParameters);

            this.CurrentView = new SelectSignalsView { DataContext = viewModel };
            this.CurrentStep = NavigationStep.SelectSignals;
        }

        public void ShowExportSettings()
        {
            this.CurrentView = new OptionsView() { DataContext = new ExportOptionsViewModel() { ExportParameters = this.ExportParameters } };
            this.CurrentStep = NavigationStep.Settings;
        }

        public void Export()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog() { FileName = "CPAP Export.csv", DefaultExt = "csv", AddExtension = true, };

            if (dialog.ShowDialog() == true)
            {
                CsvExporter exporter = new(
                    [.. this.exportParameters.Reports.Where(r => r.IsSelected).Select(r => r.DailyReport)],
                    this.exportParameters.SignalNames
                );

                string filename = this.exportParameters.DestinationPath = dialog.FileName;

                string filePart = Path.GetFileNameWithoutExtension(filename);
                string eventsFilename = Path.Combine(Path.GetDirectoryName(filename), $"{filePart} (events).csv");

                exporter.ExportToFile(filename);
                exporter.ExportFlaggedEventsToFile(eventsFilename);

                this.CurrentStep = NavigationStep.Export;
                this.CurrentView = new SavedFilesView();

                var savedVM = (SavedFilesViewModel)this.CurrentView.DataContext;

                string minDate = this.exportParameters.Reports.Where(r => r.IsSelected).Min(r => r.DailyReport.ReportDate).ToString("yyyy-MM-dd");
                string maxDate = this.exportParameters.Reports.Where(r => r.IsSelected).Max(r => r.DailyReport.ReportDate).ToString("yyyy-MM-dd");

                string date = minDate;
                if (minDate != maxDate)
                {
                    date = $"{minDate} to {maxDate}";
                }

                savedVM.AddFile(filename, $"CSV with signal data from {date}");
                savedVM.AddFile(eventsFilename, $"Events CSV from {date}");
            }
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

        private void Subscribe(ViewModel viewModel)
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void Unsubscribe(ViewModel viewModel)
        {
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.CurrentView));
        }

        #endregion

        #endregion
    }
}
