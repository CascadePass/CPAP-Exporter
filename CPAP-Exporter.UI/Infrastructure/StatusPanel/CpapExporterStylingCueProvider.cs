using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class CpapExporterStylingCueProvider : DefaultStylingCueProvider
    {
        public override Brush GetStatusPanelBorderBrush(IStylingCue message)
        {
            return message?.ContentType switch
            {
                CuedContentType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.MessageBorderBrush"),
                CuedContentType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.MessageBorderBrush"),
                CuedContentType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.MessageBorderBrush"),
                CuedContentType.Busy => ResourceLocator.GetResource<Brush>("ControlElevationBorderBrush"),
                CuedContentType.Custom => Brushes.Transparent,
                _ => base.GetStatusPanelBorderBrush(message), // Fallback to base implementation
            };
        }

        public override Brush GetBackgroundBrush(IStylingCue message)
        {
            return message?.ContentType switch
            {
                CuedContentType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.Background"),
                CuedContentType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.Background"),
                CuedContentType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.Background"),
                CuedContentType.Busy => ResourceLocator.GetResource<Brush>("Legibility.Background"),
                CuedContentType.Custom => ResourceLocator.GetResource<Brush>("Legibility.Background"),
                _ => ResourceLocator.GetResource<Brush>("Legibility.Background")
            };
        }

        public override Brush GetForegroundBrush(IStylingCue message)
        {
            return message?.ContentType switch
            {
                CuedContentType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.Foreground"),
                CuedContentType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.Foreground"),
                CuedContentType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.Foreground"),
                CuedContentType.Busy => Brushes.Transparent,
                CuedContentType.Custom => Brushes.Transparent,
                _ => base.GetStatusPanelBorderBrush(message), // Fallback to base implementation
            };
        }

        public override Brush GetAttentionStripeBrush(IStylingCue message)
        {
            return message?.ContentType switch
            {
                CuedContentType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.AttentionStripeBrush"),
                CuedContentType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.AttentionStripeBrush"),
                CuedContentType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.AttentionStripeBrush"),
                CuedContentType.Busy => Brushes.Transparent,
                CuedContentType.Custom => Brushes.Transparent,
                _ => base.GetStatusPanelBorderBrush(message), // Fallback to base implementation
            };
        }

        public override Color GetShadowColor(IStylingCue message)
        {
            return (Color)ResourceLocator.GetColorResource("StatusPanel.Shadow.Color");
        }
    }
}
