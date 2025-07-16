using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class OpenFilesViewModel : PageViewModel
    {
        #region Fields

        private bool clearReportsBeforeAdding, isValid;
        private DelegateCommand browseCommand, openCommand;

        #endregion

        #region Constructors

        public OpenFilesViewModel() : base(Resources.PageTitle_OpenFiles, Resources.PageDesc_OpenFiles)
        {
        }

        public OpenFilesViewModel(ExportParameters exportParameters) : this()
        {
            this.ExportParameters = exportParameters;
            this.ClearReportsBeforeAdding = exportParameters.UserPreferences.ClearFilesBeforeAddingMore;
        }

        #endregion

        #region Properties

        public int ButtonMinimumHeight => 100;

        public List<string> Folders => this.ExportParameters.UserPreferences.RecentlyUsedFolders;

        public bool ClearReportsBeforeAdding
        {
            get => this.clearReportsBeforeAdding;
            set => this.SetPropertyValue(ref this.clearReportsBeforeAdding, value, nameof(this.ClearReportsBeforeAdding));
        }

        public bool IsFlowReductionDescriptionExpanded
        {
            get => this.ExportParameters.UserPreferences.IsFlowReductionDescriptionExpanded;
            set
            {
                if (this.ExportParameters.UserPreferences.IsFlowReductionDescriptionExpanded != value)
                {
                    this.ExportParameters.UserPreferences.IsFlowReductionDescriptionExpanded = value;
                    this.OnPropertyChanged(nameof(this.IsFlowReductionDescriptionExpanded));
                }
            }
        }

        public bool CreateCustomEvents
        {
            get => this.ExportParameters.UserPreferences.GenerateFlowEvents;
            set
            {
                if (this.ExportParameters.UserPreferences.GenerateFlowEvents != value)
                {
                    this.ExportParameters.UserPreferences.GenerateFlowEvents = value;
                    this.OnPropertyChanged(nameof(this.CreateCustomEvents));
                    this.OnPropertyChanged(nameof(this.FlowReductionBorderBrush));
                }
            }
        }

        public Brush FlowReductionBorderBrush
        {
            get
            {
                return this.ExportParameters.UserPreferences.GenerateFlowEvents ? Application.Current.Resources["OpenFiles.FlowReductionPanel.Checked.Border"] as Brush : Application.Current.Resources["OpenFiles.FlowReductionPanel.Unchecked.Border"] as Brush;
            }
        }

        public string FlowReductionImageUri
        {
            get
            {
                var theme = new CpapExporterThemeDetector().GetThemeType();

                if (theme == ThemeType.Light)
                {
                    return "/Images/FlowReduction.Light.png";
                }

                return "/Images/FlowReduction.Dark.png";
            }
        }

        #region Buttons

        public ICommand BrowseCommand => this.browseCommand ??= new(this.BrowseToOpen);

        public ICommand OpenCommand => this.openCommand ??= new(this.OpenFolder);

        #endregion

        #endregion

        #region Methods

        #region Buttons

        private void BrowseToOpen()
        {
            this.BrowseAndLoad();
        }

        private void OpenFolder(object state)
        {
            string folder = state as string;

            if (!string.IsNullOrWhiteSpace(folder))
            {
                this.Load(folder);
            }
        }

        #endregion

        public void BrowseAndLoad()
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog();

            if (dialog.ShowDialog() == true)
            {
                this.Load(dialog.FolderName);
            }
        }

        public void Load(string folder)
        {
            this.isValid = false;

            #region Validate Path

            ArgumentNullException.ThrowIfNull(folder, nameof(folder));

            if (!this.CanImportFrom(folder))
            {
                folder = this.FindImportableParentFolder(folder);
            }

            if (!this.CanImportFrom(folder))
            {
                return;
            }

            #endregion

            this.ExportParameters.SourcePath = folder;
            this.ExportParameters.UserPreferences.AddFolder(folder);
            this.isValid = true;
            this.OnPropertyChanged(nameof(this.IsValid));

            if (this.ClearReportsBeforeAdding)
            {
                this.ExportParameters.Reports.Clear();
            }

            ApplicationComponentProvider.Status.StatusText = string.Format(Resources.ReadingFolder, folder);

            this.OnAdvancePage();
        }

        internal bool CanImportFrom(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                // No reason to do any I/O or talk to the file system if there's no folder.
                return false;
            }

            if (!Directory.Exists(folder))
            {
                ApplicationComponentProvider.Status.StatusText = string.Format(Resources.FolderDoesNotExist, folder);
                return false;
            }

            if (!ApplicationComponentProvider.CpapSourceValidator.IsCpapFolderStructure(folder))
            {
                ApplicationComponentProvider.Status.StatusText = string.Format(Resources.FolderIsNotPAP, folder);
                return false;
            }

            return true;
        }

        internal string FindImportableParentFolder(string folder)
        {
            string path = folder;

            while (path is not null)
            {
                if (ApplicationComponentProvider.CpapSourceValidator.IsCpapFolderStructure(path))
                {
                    return path;
                }

                DirectoryInfo dir = new(path);

                if (dir.Parent is null)
                {
                    ApplicationComponentProvider.Status.StatusText = string.Format(Resources.NoPapData, folder);
                    return null;
                }

                path = dir.Parent.FullName;
            }

            return path;
        }

        public override bool Validate()
        {
            return base.Validate() && this.isValid;
        }

        #endregion
    }
}
