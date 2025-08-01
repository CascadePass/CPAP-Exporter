using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class CpapExporterStylingCueProvider : DefaultStylingCueProvider
    {
        public override Brush GetStatusPanelBorderBrush(IStylingCue auraContent)
        {
            return auraContent?.ContentType switch
            {
                CuedContentType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.ContentBorderBrush"),
                CuedContentType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.ContentBorderBrush"),
                CuedContentType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.ContentBorderBrush"),
                CuedContentType.Busy => ResourceLocator.GetResource<Brush>("ControlElevationBorderBrush"),
                CuedContentType.Custom => Brushes.Transparent,
                _ => base.GetStatusPanelBorderBrush(auraContent), // Fallback to base implementation
            };
        }

        public override Brush GetBackgroundBrush(IStylingCue auraContent)
        {
            return auraContent?.ContentType switch
            {
                CuedContentType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.Background"),
                CuedContentType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.Background"),
                CuedContentType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.Background"),
                CuedContentType.Busy => ResourceLocator.GetResource<Brush>("Legibility.Background"),
                CuedContentType.Custom => ResourceLocator.GetResource<Brush>("Legibility.Background"),
                _ => ResourceLocator.GetResource<Brush>("Legibility.Background")
            };
        }

        public override Brush GetForegroundBrush(IStylingCue auraContent)
        {
            return auraContent?.ContentType switch
            {
                CuedContentType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.Foreground"),
                CuedContentType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.Foreground"),
                CuedContentType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.Foreground"),
                CuedContentType.Busy => Brushes.Transparent,
                CuedContentType.Custom => Brushes.Transparent,
                _ => base.GetStatusPanelBorderBrush(auraContent), // Fallback to base implementation
            };
        }

        public override Brush GetAttentionStripeBrush(IStylingCue auraContent)
        {
            return auraContent?.ContentType switch
            {
                CuedContentType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.AttentionStripeBrush"),
                CuedContentType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.AttentionStripeBorderBrush"),
                CuedContentType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.AttentionStripeBrush"),
                CuedContentType.Busy => Brushes.Transparent,
                CuedContentType.Custom => Brushes.Transparent,
                _ => base.GetStatusPanelBorderBrush(auraContent), // Fallback to base implementation
            };
        }

        public override double GetAttentionStripeWidth(IStylingCue auraContent)
        {
            return 6;
        }

        public override Color GetShadowColor(IStylingCue auraContent)
        {
            return (Color)ResourceLocator.GetColorResource("StatusPanel.Shadow.Color");
        }

        public override Thickness GetBorderThickness(IStylingCue auraContent)
        {
            return new Thickness(2);
        }
    }
}
