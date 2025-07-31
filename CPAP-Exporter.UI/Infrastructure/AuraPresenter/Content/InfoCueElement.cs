using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class InfoCueElement : IconCueElement
    {
        public InfoCueElement() : base(null, CuedContentType.Info) { }

        public InfoCueElement(object displayContent) : base(displayContent, CuedContentType.Info) { }

        public InfoCueElement(object displayContent, CuedContentType cuedContentType, ImageSource iconSource) : base(displayContent, cuedContentType, iconSource) { }
    }
}
