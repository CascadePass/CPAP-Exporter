using CascadePass.CPAPExporter.Core;

namespace CascadePass.CPAPExporter
{
    public class SignalViewModel : ViewModel
    {
        private bool isSelected;
        private SignalInfo signalInfo;

        public SignalViewModel(SignalInfo signalDetails) {
            this.IsSelected = true;
            this.SignalInfo = signalDetails;
        }

        public bool IsSelected
        {
            get => this.isSelected;
            set => this.SetPropertyValue(ref this.isSelected, value, nameof(this.IsSelected));
        }

        public SignalInfo SignalInfo
        {
            get => this.signalInfo;
            set => this.SetPropertyValue(ref this.signalInfo, value, nameof(this.SignalInfo));
        }
    }
}
