using System.Windows;
using System.Windows.Controls;

namespace CascadePass.CPAPExporter
{
    public class AuraTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }

        public DataTemplate IconCueTemplate { get; set; }

        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IconCueElement)
            {
                return this.IconCueTemplate;
            }

            if(item is string)
            {
                return this.StringTemplate;
            }

            return this.DefaultTemplate;
        }
    }
}
