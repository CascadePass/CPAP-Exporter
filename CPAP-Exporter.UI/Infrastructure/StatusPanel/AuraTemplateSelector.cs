using System.Windows;
using System.Windows.Controls;

namespace CascadePass.CPAPExporter
{
    public class AuraTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IconCueElement { get; set; }

        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IconCueElement)
            {
                return this.IconCueElement;
            }

            return this.DefaultTemplate;
        }
    }
}
