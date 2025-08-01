using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class BusyToast : IconToast
    {
        public BusyToast() : base(null, CuedContentType.Busy) {
            this.Content = this.ProgressBar = this.CreateProgressBar();

            // BusyToast is centered by default, an AttentionStripe ruins this effect.
            this.AttentionStripeCue = new() {
                Brush = Brushes.Transparent,
                Width = 0
            };
        }

        public ProgressBar ProgressBar { get; set; }

        internal ProgressBar CreateProgressBar()
        {
            return new ProgressBar
            {
                IsIndeterminate = true,
                Visibility = Visibility.Visible,
                Height = 24
            };
        }
    }
}
