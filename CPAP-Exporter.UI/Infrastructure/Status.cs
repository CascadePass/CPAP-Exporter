namespace CascadePass.CPAPExporter
{
    public class Status : Observable, IStatus
    {
        private string statusText;

        public string StatusText
        {
            get => this.statusText;
            set => this.SetPropertyValue(ref this.statusText, value, nameof(this.StatusText));
        }
    }
}
