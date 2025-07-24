using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Defines a provider for default values determining the visual styles and behaviors of status messages.
    /// </summary>
    /// <remarks>This interface allows customization of the appearance and animation of status messages by
    /// providing brushes, dimensions, and other style-related properties based on the characteristics of
    /// a given <see cref="IStatusMessage"/>.</remarks>
    public interface IStatusMessageStyleProvider
    {
        Brush GetAttentionStripeBrush(IStatusMessage message);
        double GetAttentionStripeWidth(IStatusMessage message);
        Brush GetBackgroundBrush(IStatusMessage message);
        Thickness GetBorderThickness(IStatusMessage message);
        double GetCornerRadius(IStatusMessage message);
        bool GetFadeIn(IStatusMessage message);
        bool GetFadeOut(IStatusMessage message);
        Brush GetForegroundBrush(IStatusMessage message);
        bool GetPulseBorder(IStatusMessage message);
        Color GetPulseEndColor(IStatusMessage message);
        Color GetPulseStartColor(IStatusMessage message);
        bool GetShowDropShadow(IStatusMessage message);
        Brush GetStatusPanelBorderBrush(IStatusMessage message);
    }
}