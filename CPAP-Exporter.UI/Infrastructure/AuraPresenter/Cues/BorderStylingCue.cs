using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class BorderStylingCue : StylingCue
    {
        public Brush BorderBrush { get; set; }
        public Brush BackgroundBrush { get; set; }
        public double? CornerRadius { get; set; }
        public Thickness? BorderThickness { get; set; }

        public override bool IsEmpty =>
            BorderBrush == null &&
            BackgroundBrush == null &&
            CornerRadius == null &&
            BorderThickness == null
        ;
    }
}
