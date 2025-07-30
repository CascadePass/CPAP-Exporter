using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class StatusPanelMessage : IStatusMessage
    {
        public StatusPanelMessage() { }

        public StatusPanelMessage(object messageContent) {
            this.MessageContent = messageContent;
        }

        public StatusPanelMessage(object messageContent, StatusMessageType messageType)
        {
            this.MessageContent = messageContent;
            this.MessageType = messageType;
        }

        public StatusMessageType MessageType { get; set; }
        public object MessageContent { get; set; }

        #region Panel Properties

        public Brush BorderBrush { get; set; }
        public Brush ForegroundBrush { get; set; }
        public Brush BackgroundBrush { get; set; }

        public Color? PulseStartColor { get; set; }
        public Color? PulseEndColor { get; set; }
        

        public Brush AttentionStripeBrush { get; set; }
        public double? AttentionStripeWidth { get; set; }

        public bool? ShowDropShadow { get; set; }
        public double? CornerRadius { get; set; }
        public Thickness? BorderThickness { get; set; }

        #endregion

        #region Animation Control Properties

        public bool? FadeMessageIn { get; set; }
        public bool? FadeMessageOut { get; set; }
        public bool? PulseBorder { get; set; }

        public TimeSpan? DisplayDuration { get; set; }

        #endregion


        #region Shadow Properties

        public Color? ShadowColor { get; set; }
        public double? ShadowOpacity { get; set; }
        public double? ShadowBlurRadius { get; set; }
        public double? ShadowDepth { get; set; }

        #endregion
    }
}
