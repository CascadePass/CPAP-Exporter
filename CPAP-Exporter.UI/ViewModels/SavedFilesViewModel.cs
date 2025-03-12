using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace CascadePass.CPAPExporter
{
    public class SavedFilesViewModel : PageViewModel
    {
        private DelegateCommand browseFolderCommand;

        public SavedFilesViewModel() : base(Resources.PageTitle_SavedFiles, Resources.PageDesc_SavedFiles)
        {
            this.Files = [];
        }

        public ObservableCollection<SavedFileViewModel> Files { get; set; }

        public ICommand BrowseFolderCommand => this.browseFolderCommand ??= new DelegateCommand(this.BrowseFolder);

        public void AddFile(string filename, string fileDescription)
        {
            if(File.Exists(filename))
            {
                SavedFileViewModel fileViewModel = new(filename, fileDescription);
                this.Files.Add(fileViewModel);

                fileViewModel.PropertyChanged += this.FileViewModel_PropertyChanged;
            }
        }

        public void BrowseFolder()
        {
            if (this.Files.Count > 0)
            {
                this.OpenFileInExplorer([.. this.Files.Select(f => f.Filename)]);
            }
        }

        public void OpenFileInExplorer(string[] filePaths)
        {
            string arguments = "/select,";

            foreach (string filePath in filePaths)
            {
                arguments += "\"" + filePath + "\",";
            }

            // Remove the trailing comma
            arguments = arguments.TrimEnd(',');

            Process.Start("explorer.exe", arguments);
        }


        private void FileViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Files));
        }
    }
}
