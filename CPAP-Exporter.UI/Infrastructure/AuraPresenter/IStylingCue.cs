using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public interface IStylingCue
    {
        CuedContentType ContentType { get; }

        object Content { get; set; }

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

        #region Control Animations

        bool? FadeContentIn { get; set; }
        bool? FadeContentOut { get; set; }
        bool? PulseBorder { get; set; }

        #endregion

        #region Shadow

        Color? ShadowColor { get; set; }
        double? ShadowOpacity { get; set; }
        double? ShadowBlurRadius { get; set; }
        double? ShadowDepth { get; set; }

        #endregion

        TimeSpan? DisplayDuration { get; set; }
    }
}
