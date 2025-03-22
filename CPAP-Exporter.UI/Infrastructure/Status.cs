namespace CascadePass.CPAPExporter
{
    public class Status : Observable, IStatus
    {
        private string statusText;
        private StatusProgressBar statusProgressBar;

        public string StatusText
        {
            get => this.statusText;
            set => this.SetPropertyValue(ref this.statusText, value, nameof(this.StatusText));
        }

        public StatusProgressBar ProgressBar
        {
            get => this.statusProgressBar;
            set => this.SetPropertyValue(ref this.statusProgressBar, value, nameof(this.ProgressBar));
        }
    }

    public class StatusProgressBar : ViewModel
    {
        private int min, max, curent;

        public StatusProgressBar(int minimmum, int maximum, int currentValue)
        {
            this.Min = minimmum;
            this.Max = maximum;
            this.Current = currentValue;
        }

        public int Min
        {
            get => this.min;
            set => this.SetPropertyValue(ref this.min, value, nameof(this.Min));
        }

        public int Max
        {
            get => this.max;
            set => this.SetPropertyValue(ref this.max, value, nameof(this.Max));
        }

        public int Current
        {
            get => this.curent;
            set => this.SetPropertyValue(ref this.curent, value, nameof(this.Current));
        }
    }
}
