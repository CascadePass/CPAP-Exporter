using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class TextStylingCue : StylingCue
    {
        public Brush ForegroundBrush { get; set; }
        public FontFamily FontFamily { get; set; }
        public double? FontSize { get; set; }
        public FontWeight? FontWeight { get; set; }
        public FontStyle? FontStyle { get; set; }

        public override bool IsEmpty =>
            ForegroundBrush == null &&
            FontFamily == null &&
            FontSize == null &&
            FontWeight == null &&
            FontStyle == null
        ;
    }
}
