using System.Windows;
using System.Windows.Controls;

namespace CascadePass.CPAPExporter
{
    public class StatusMessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate InfoTemplate { get; set; }
        public DataTemplate WarningTemplate { get; set; }
        public DataTemplate ErrorTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ContentStylingCue stylingCue)
            {
                return stylingCue.MessageType switch
                {
                    StatusMessageType.Info => InfoTemplate,
                    StatusMessageType.Warning => WarningTemplate,
                    StatusMessageType.Error => ErrorTemplate,
                    _ => DefaultTemplate
                };
            }

            // If it's not a StatusPanelMessage, return null to let WPF render naturally
            return null;
        }
    }
}
