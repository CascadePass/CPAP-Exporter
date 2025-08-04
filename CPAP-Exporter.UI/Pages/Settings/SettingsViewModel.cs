using System.Collections.ObjectModel;
using System.Windows;

namespace CascadePass.CPAPExporter
{
    public class SettingsViewModel : PageViewModel
    {
        public SettingsViewModel() : base(Resources.PageTitle_Settings, Resources.PageDesc_Settings)
        {
            this.CreateAcknowledgements();
        }

        public ObservableCollection<Acknowledgement> Acknowledgements { get; private set; }

        public double FontSize
        {
            get => this.ExportParameters.UserPreferences.FontSize;
            set
            {
                if (this.ExportParameters.UserPreferences.FontSize != value)
                {
                    if (Application.Current?.MainWindow is not null)
                    {
                        Application.Current.MainWindow.FontSize = value;
                    }

                    this.ExportParameters.UserPreferences.FontSize = value;
                    this.OnPropertyChanged(nameof(this.FontSize));
                }
            }
        }

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
