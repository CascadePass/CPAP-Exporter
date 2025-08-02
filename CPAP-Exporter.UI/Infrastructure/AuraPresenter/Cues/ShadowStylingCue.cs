using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class ShadowStylingCue : StylingCue
    {
        public Color? ShadowColor { get; set; }
        public double? ShadowOpacity { get; set; }
        public double? ShadowBlurRadius { get; set; }
        public double? ShadowDepth { get; set; }

        public override bool IsEmpty =>
            ShadowColor == null &&
            ShadowOpacity == null &&
            ShadowBlurRadius == null &&
            ShadowDepth == null
        ;
    }
}
