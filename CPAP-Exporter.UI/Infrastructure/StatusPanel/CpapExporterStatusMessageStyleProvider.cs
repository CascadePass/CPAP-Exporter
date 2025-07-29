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
                _ => base.GetStatusPanelBorderBrush(message), // Fallback to base implementation
            };
        }

        public override Brush GetForegroundBrush(IStatusMessage message)
        {
            return message?.MessageType switch
            {
                StatusMessageType.Info => ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.Foreground"),
                StatusMessageType.Warning => ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.Foreground"),
                StatusMessageType.Error => ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.Foreground"),
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
                _ => base.GetStatusPanelBorderBrush(message), // Fallback to base implementation
            };
        }
    }
}
