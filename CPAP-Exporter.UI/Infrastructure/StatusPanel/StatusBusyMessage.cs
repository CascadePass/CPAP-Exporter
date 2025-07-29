using System.Windows;
using System.Windows.Controls;

namespace CascadePass.CPAPExporter
{
    public class StatusBusyMessage : StatusPanelMessage
    {
        public StatusBusyMessage() : base(null, StatusMessageType.Busy) {
            this.ProgressBar = this.CreateProgressBar();
            this.MessageContent = this.ProgressBar;
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
