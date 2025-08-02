using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class AttentionStripeCue : StylingCue
    {
        public Brush Brush { get; set; }
        public double? Width { get; set; }

        public override bool IsEmpty =>
            Brush != null &&
            Width != null
        ;
    }
}
