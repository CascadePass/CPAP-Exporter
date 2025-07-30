using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class CpapExporterStatusMessageStyleProvider : DefaultStatusMessageStyleProvider
    {
        public override Brush GetStatusPanelBorderBrush(IStatusMessage message)
        {
            return message?.MessageType switch
            {
                StatusMessageType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.MessageBorderBrush"),
                StatusMessageType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.MessageBorderBrush"),
                StatusMessageType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.MessageBorderBrush"),
                StatusMessageType.Busy => ResourceLocator.GetResource<Brush>("ControlElevationBorderBrush"),
                StatusMessageType.Custom => Brushes.Transparent,
                _ => base.GetStatusPanelBorderBrush(message), // Fallback to base implementation
            };
        }

        public override Brush GetBackgroundBrush(IStatusMessage message)
        {
            return message?.MessageType switch
            {
                StatusMessageType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.Background"),
                StatusMessageType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.Background"),
                StatusMessageType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.Background"),
                StatusMessageType.Busy => ResourceLocator.GetResource<Brush>("Legibility.Background"),
                StatusMessageType.Custom => ResourceLocator.GetResource<Brush>("Legibility.Background"),
                _ => ResourceLocator.GetResource<Brush>("Legibility.Background")
            };
        }

        public override Brush GetForegroundBrush(IStatusMessage message)
        {
            return message?.MessageType switch
            {
                StatusMessageType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.Foreground"),
                StatusMessageType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.Foreground"),
                StatusMessageType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.Foreground"),
                StatusMessageType.Busy => Brushes.Transparent,
                StatusMessageType.Custom => Brushes.Transparent,
                _ => base.GetStatusPanelBorderBrush(message), // Fallback to base implementation
            };
        }

        public override Brush GetAttentionStripeBrush(IStatusMessage message)
        {
            return message?.MessageType switch
            {
                StatusMessageType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.AttentionStripeBrush"),
                StatusMessageType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.AttentionStripeBrush"),
                StatusMessageType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.AttentionStripeBrush"),
                StatusMessageType.Busy => Brushes.Transparent,
                StatusMessageType.Custom => Brushes.Transparent,
                _ => base.GetStatusPanelBorderBrush(message), // Fallback to base implementation
            };
        }

        public override Color GetShadowColor(IStatusMessage message)
        {
            return (Color)ResourceLocator.GetColorResource("StatusPanel.Shadow.Color");
        }
    }
}
