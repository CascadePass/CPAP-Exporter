using System.Collections.ObjectModel;

namespace CascadePass.CPAPExporter
{
    public class HashesViewModel : PageViewModel
    {
        private bool includeSystemModules;
        private NavigationViewModel navViewModel;

        public HashesViewModel(NavigationViewModel navigationViewModel) : base(Resources.PageTitle_Hashes, Resources.PageDesc_Hashes)
        {
            this.navViewModel = navigationViewModel;
            this.FileHashes = [];
            this.GetFileHashes();
        }

        public ObservableCollection<KeyValuePair<string, string>> FileHashes { get; set; }

        public bool IncludeSystemModules
        {
            get => this.includeSystemModules;
            set
            {
                bool wasUpdated = this.SetPropertyValue(ref this.includeSystemModules, value, nameof(this.IncludeSystemModules));

                if (wasUpdated)
                {
                    this.GetFileHashes();
                }
            }
        }

        internal void GetFileHashes()
        {
            this.FileHashes.Clear();

            foreach (var kvp in ApplicationHashCalculator.CalculateHashes(this.IncludeSystemModules))
            {
                this.FileHashes.Add(kvp);
            }
        }
    }
}
