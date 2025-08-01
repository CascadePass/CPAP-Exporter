using System.Windows;
using System.Windows.Controls;

namespace CascadePass.CPAPExporter
{
    public class AuraTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }

        public DataTemplate IconToastTemplate { get; set; }

        public DataTemplate BusyToastTemplate { get; set; }

        public DataTemplate CardTemplate { get; set; }

        public DataTemplate DefaultTemplate { get; set; }

        

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is BusyToast)
            {
                return this.BusyToastTemplate;
            }

            if (item is IconToast)
            {
                return this.IconToastTemplate;
            }

            if (item is IconToast)
            {
                return this.IconToastTemplate;
            }

            if (item is Card)
            {
                return this.CardTemplate;
            }

            return this.DefaultTemplate;
        }
    }
}
