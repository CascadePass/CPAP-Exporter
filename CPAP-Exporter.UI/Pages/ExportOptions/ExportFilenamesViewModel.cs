namespace CascadePass.CPAPExporter
{
    public class ExportFilenamesViewModel : ViewModel
    {
        private string label, rawFilename, eventsFilename;

        public string Label
        {
            get => this.label;
            set => this.SetPropertyValue(ref this.label, value, nameof(this.Label));
        }

        public string RawFilename
        {
            get => this.rawFilename;
            set => this.SetPropertyValue(ref this.rawFilename, value, nameof(this.RawFilename));
        }

        public string EventsFilename
        {
            get => this.eventsFilename;
            set => this.SetPropertyValue(ref this.eventsFilename, value, nameof(this.EventsFilename));
        }
    }
}
