using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class ErrorCueElement : IconCueElement
    {
        public ErrorCueElement() : base(null, CuedContentType.Error) { }

        public ErrorCueElement(object messageContent) : base(messageContent, CuedContentType.Error) { }

        public ErrorCueElement(object messageContent, CuedContentType cuedContentType, ImageSource iconSource) : base(messageContent, cuedContentType, iconSource) { }
    }
}
