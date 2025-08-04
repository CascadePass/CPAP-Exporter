using System.Collections.ObjectModel;

namespace CascadePass.CPAPExporter
{
    public class SettingsViewModel : PageViewModel
    {
        public SettingsViewModel() : base(Resources.PageTitle_Settings, Resources.PageDesc_Settings)
        {
            this.CreateAcknowledgements();
        }

        public ObservableCollection<Acknowledgement> Acknowledgements { get; private set; }

        private void CreateAcknowledgements()
        {
            this.Acknowledgements = [
                new() { Title = "EEGKit", Url = "https://github.com/EEGKit/cpap-lib" },
                new() { Title = "StagPoint", Url  = "https://github.com/EEGKit/StagPoint.EuropeanDataFormat.Net" },
                new() { Title = "Json.net", Url  = "https://www.newtonsoft.com/json" },
            ];
        }
    }
}
