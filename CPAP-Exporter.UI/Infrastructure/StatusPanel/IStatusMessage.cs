using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public interface IStatusMessage
    {
        StatusMessageType MessageType { get; }

        object MessageContent { get; set; }

        #region Panel

        Brush BorderBrush { get; set; }
        Brush ForegroundBrush { get; set; }
        Brush BackgroundBrush { get; set; }
        Brush AttentionStripeBrush { get; set; }

        Color? PulseStartColor { get; set; }
        Color? PulseEndColor { get; set; }
        
        double? AttentionStripeWidth { get; set; }

        bool? ShowDropShadow { get; set; }

        double? CornerRadius { get; set; }
        Thickness? BorderThickness { get; set; }

        #endregion

        #region Message

        bool? FadeMessageIn { get; set; }
        bool? FadeMessageOut { get; set; }
        bool? PulseBorder { get; set; }

        #endregion
    }
}
