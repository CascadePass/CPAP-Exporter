using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class ErrorCueElement : IconCueElement
    {
        public ErrorCueElement() : base(null, CuedContentType.Error) { }

        public ErrorCueElement(object displayContent) : base(displayContent, CuedContentType.Error) { }

        public ErrorCueElement(object displayContent, CuedContentType cuedContentType, ImageSource iconSource) : base(displayContent, cuedContentType, iconSource) { }
    }
}
