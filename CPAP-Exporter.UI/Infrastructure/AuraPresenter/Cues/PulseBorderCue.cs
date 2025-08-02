using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class PulseBorderCue : AnimationStylingCue
    {
        public Color FromColor { get; set; }
        public Color ToColor { get; set; }

        public override bool IsEmpty =>
            Duration == default &&
            FromColor == ToColor
        ;
    }
}
