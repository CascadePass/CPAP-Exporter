using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CascadePass.CPAPExporter
{
    public class SavedFileViewModel : ViewModel
    {
        private bool isDeleted;
        private string name, desc;
        private Brush backgroundImageBrush;
        private SavedFileType type;
        private DelegateCommand browseCommand, deleteCommand, launchCommand;

        private static Brush fullFileBrush, eventsFileBrush;

        public event EventHandler<EventArgs> FileDeleted;

        public SavedFileViewModel(string filename, string description, SavedFileType fileType)
        {
            this.Filename = filename;
            this.Description = description;
            this.FileType = fileType;

            if (fileType == SavedFileType.FullExport)
            {
                this.BackgroundImageBrush = SavedFileViewModel.fullFileBrush;
            }
            else if(fileType == SavedFileType.EventsExport)
            {
                this.BackgroundImageBrush = SavedFileViewModel.eventsFileBrush;
            }

            if (this.FileInfo is null || !this.FileInfo.Exists)
            {
                throw new ArgumentOutOfRangeException(nameof(filename));
            }
        }

        static SavedFileViewModel()
        {
            SavedFileViewModel.fullFileBrush = SavedFileViewModel.CreateImageBrush("/Images/SavedFiles.RawFilePanel.Background.png");
            SavedFileViewModel.eventsFileBrush = SavedFileViewModel.CreateImageBrush("/Images/SavedFiles.EventsFilePanel.Background.png");
        }

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

                ApplicationComponentProvider.Status.StatusText = string.Format(Resources.FileWasDeleted, this.Filename);
            }
            catch (Exception ex)
            {
                ApplicationComponentProvider.Status.StatusText = ex.Message;
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


        public static ImageBrush CreateImageBrush(string resourceRelativePath)
        {
            var uri = new Uri($"pack://application:,,,/{resourceRelativePath}", UriKind.Absolute);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            return new ImageBrush(bitmap)
            {
                Stretch = Stretch.Fill,
            };
        }

        protected void OnFileDeleted(object sender, EventArgs e)
        {
            this.FileDeleted?.Invoke(this, e);
        }
    }
}
