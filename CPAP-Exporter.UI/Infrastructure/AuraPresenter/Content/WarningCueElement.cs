using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class WarningCueElement : IconCueElement
    {
        public WarningCueElement() : base(null, CuedContentType.Warning) { }

        public WarningCueElement(object displayContent) : base(displayContent, CuedContentType.Warning) { }

        public WarningCueElement(object displayContent, CuedContentType cuedContentType, ImageSource iconSource) : base(displayContent, cuedContentType, iconSource) { }
    }
}
