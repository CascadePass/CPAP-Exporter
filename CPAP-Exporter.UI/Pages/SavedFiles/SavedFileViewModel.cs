using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CascadePass.CPAPExporter
{
    public class SavedFileViewModel : ViewModel
    {
        #region Private fields

        private bool isDeleted;
        private string name, desc;
        private Brush backgroundImageBrush;
        private SavedFileType type;
        private DelegateCommand browseCommand, deleteCommand, launchCommand;

        private static readonly Brush fullFileBrush;
        private static Brush eventsFileBrush;

        #endregion

        public event EventHandler<EventArgs> FileDeleted;

        #region Constructors

        public SavedFileViewModel(string filename, string description, SavedFileType fileType)
        {
            this.Filename = filename;
            this.Description = description;
            this.FileType = fileType;

            this.AssignBrush();

            // FileInfo is created when Filename is assigned.
            if (this.FileInfo is null || !this.FileInfo.Exists)
            {
                throw new ArgumentOutOfRangeException(nameof(filename));
            }
        }

        /// <summary>
        /// Initializes static members of the <see cref="SavedFileViewModel"/> class.
        /// </summary>
        /// <remarks>This static constructor sets up the default image brushes used for the file panels.
        /// It is automatically invoked before any static members of the class are accessed.</remarks>
        static SavedFileViewModel()
        {
            // Test whether there is a valid WPF Application context before trying to load resources.

            if (Application.Current is not null)
            {
                // Cache image brushes which will be reused for each file CPAP-Exporter generates.

                SavedFileViewModel.fullFileBrush = SavedFileViewModel.CreateImageBrush("/Images/SavedFiles.RawFilePanel.Background.png");
                SavedFileViewModel.eventsFileBrush = SavedFileViewModel.CreateImageBrush("/Images/SavedFiles.EventsFilePanel.Background.png");
            }
            else
            {
                // If Application.Current is null, we are likely in a unit test or non-UI context.
                // We can still create the brushes, but they won't be used in the UI.  These are
                // being created to avoid destabilizing downstream code with null references.

                SavedFileViewModel.fullFileBrush = new SolidColorBrush(Colors.Red);
                SavedFileViewModel.eventsFileBrush = new SolidColorBrush(Colors.Red);

                // Hopefully red brushes will make this obvious, if somehow this comes up in a UI context.
            }
        }

        #endregion

        #region Properties

        public string Filename {
            get => this.name;
            set
            {
                if(this.SetPropertyValue(ref this.name, value, nameof(this.Filename)))
                {
                    this.FileInfo = new(value);
                }
            }
        }

        public string Description
        {
            get => this.desc;
            set => this.SetPropertyValue(ref this.desc, value, nameof(this.Description));
        }

        public bool IsDeleted
        {
            get => this.isDeleted;
            set => this.SetPropertyValue(ref this.isDeleted, value, nameof(this.IsDeleted));
        }

        public SavedFileType FileType
        {
            get => this.type;
            set => this.SetPropertyValue(ref this.type, value, nameof(this.FileType));
        }

        public Brush BackgroundImageBrush
        {
            get => this.backgroundImageBrush;
            set => this.SetPropertyValue(ref this.backgroundImageBrush, value, nameof(this.BackgroundImageBrush));
        }

        public FileInfo FileInfo { get; set; }

        public ICommand BrowseCommand => this.browseCommand ??= new(this.BrowseToFile);
        public ICommand DeleteCommand => this.deleteCommand ??= new(this.DeleteFile);
        public ICommand LaunchCommand => this.launchCommand ??= new(this.LaunchFile);

        #endregion

        public void BrowseToFile()
        {
            if(this.isDeleted && this.FileInfo.Exists)
            {
                return;
            }

            WindowsExplorerUtility.BrowseToFile(this.Filename);
        }

        public void DeleteFile()
        {
            if (this.isDeleted && this.FileInfo.Exists)
            {
                return;
            }

            try
            {
                this.FileInfo.Delete();
                this.IsDeleted = true;
                this.OnFileDeleted(this, EventArgs.Empty);

                //ApplicationComponentProvider.Status.StatusText = string.Format(Resources.FileWasDeleted, this.Filename);
            }
            catch (Exception ex)
            {
                //ApplicationComponentProvider.Status.StatusText = ex.Message;
                var window = Application.Current?.MainWindow;
                var viewModel = window?.DataContext as PageViewModel;

                Application.Current.Dispatcher.Invoke(() => { viewModel.StatusContent = new ErrorToast(ex.Message); });
            }
        }

        public void LaunchFile()
        {
            if (this.isDeleted && this.FileInfo.Exists)
            {
                return;
            }

            WindowsExplorerUtility.LaunchFile(this.Filename);
        }

        internal void AssignBrush()
        {
            if (this.FileType == SavedFileType.FullExport)
            {
                this.BackgroundImageBrush = SavedFileViewModel.fullFileBrush;
            }
            else if (this.FileType == SavedFileType.EventsExport)
            {
                this.BackgroundImageBrush = SavedFileViewModel.eventsFileBrush;
            }
        }

        public static ImageBrush CreateImageBrush(string resourceRelativePath)
        {
            var uri = new Uri($"pack://application:,,,{resourceRelativePath}", UriKind.Absolute);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            return new ImageBrush(bitmap)
            {
                Stretch = Stretch.Fill,
                Opacity = 0.5,
            };
        }

        protected void OnFileDeleted(object sender, EventArgs e)
        {
            this.FileDeleted?.Invoke(this, e);
        }
    }
}
