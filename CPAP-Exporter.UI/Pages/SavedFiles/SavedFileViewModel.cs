using System.IO;
using System.Windows.Input;

namespace CascadePass.CPAPExporter
{
    public class SavedFileViewModel : ViewModel
    {
        private bool isDeleted;
        private string name, desc;
        private DelegateCommand browseCommand, deleteCommand, launchCommand;

        public event EventHandler<EventArgs> FileDeleted;

        public SavedFileViewModel(string filename, string description)
        {
            this.Filename = filename;
            this.Description = description;

            if (this.FileInfo is null || !this.FileInfo.Exists)
            {
                throw new ArgumentOutOfRangeException(nameof(filename));
            }
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

        protected void OnFileDeleted(object sender, EventArgs e)
        {
            this.FileDeleted?.Invoke(this, e);
        }
    }
}
