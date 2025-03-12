using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace CascadePass.CPAPExporter
{
    public class SavedFileViewModel : ViewModel
    {
        private string name, desc;
        private DelegateCommand browseCommand, deleteCommand, launchCommand;

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

        public FileInfo FileInfo { get; set; }

        public ICommand BrowseCommand => this.browseCommand ??= new(this.BrowseToFile);
        public ICommand DeleteCommand => this.deleteCommand ??= new(this.DeleteFile);
        public ICommand LaunchCommand => this.launchCommand ??= new(this.LaunchFile);

        public void BrowseToFile()
        {
            WindowsExplorerUtility.BrowseToFile(this.Filename);
        }

        public void DeleteFile()
        {
            this.FileInfo.Delete();
        }

        public void LaunchFile()
        {
            WindowsExplorerUtility.LaunchFile(this.Filename);
        }
    }
}
