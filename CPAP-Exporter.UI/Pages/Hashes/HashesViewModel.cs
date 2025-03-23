using System.Windows;
using System.Windows.Input;

namespace CascadePass.CPAPExporter
{
    public class HashesViewModel : PageViewModel
    {
        private NavigationViewModel navViewModel;
        private DelegateCommand closeCommand;

        public HashesViewModel(NavigationViewModel navigationViewModel) : base(Resources.PageTitle_OpenFiles, Resources.PageDesc_OpenFiles)
        {
            this.navViewModel = navigationViewModel;
            this.FileHashes = [.. ApplicationHashCalculator.CalculateHashes()];
        }

        public List<KeyValuePair<string, string>> FileHashes { get; set; }

        public FrameworkElement PreviewView { get; set; }

        public ICommand CloseCommand => this.closeCommand ??= new DelegateCommand(this.Close);

        private void Close()
        {
            this.navViewModel.CurrentView = this.PreviewView;
        }
    }
}
