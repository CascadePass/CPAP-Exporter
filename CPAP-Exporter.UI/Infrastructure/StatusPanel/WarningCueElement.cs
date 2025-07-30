using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class WarningCueElement : IconCueElement
    {
        public WarningCueElement() : base(null, CuedContentType.Warning) { }

        public WarningCueElement(object messageContent) : base(messageContent, CuedContentType.Warning) { }

        public WarningCueElement(object messageContent, CuedContentType cuedContentType, ImageSource iconSource) : base(messageContent, cuedContentType, iconSource) { }
    }
}
