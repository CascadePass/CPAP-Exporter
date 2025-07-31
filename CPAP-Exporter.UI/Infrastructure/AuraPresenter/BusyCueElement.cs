using System.Windows;
using System.Windows.Controls;

namespace CascadePass.CPAPExporter
{
    public class BusyCueElement : ContentStylingCue
    {
        public BusyCueElement() : base(null, CuedContentType.Busy) {
            this.ProgressBar = this.CreateProgressBar();
            this.Content = this.ProgressBar;
            this.AttentionStripeWidth = 0;
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
