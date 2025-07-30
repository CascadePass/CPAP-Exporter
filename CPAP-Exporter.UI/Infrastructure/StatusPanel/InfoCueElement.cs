using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class InfoCueElement : IconCueElement
    {
        public InfoCueElement() : base(null, CuedContentType.Info) { }

        public InfoCueElement(object messageContent) : base(messageContent, CuedContentType.Info) { }

        public InfoCueElement(object messageContent, CuedContentType cuedContentType, ImageSource iconSource) : base(messageContent, cuedContentType, iconSource) { }
    }
}
