using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class WarningToast : IconToast
    {
        public WarningToast() : base(null, CuedContentType.Warning) { }

        public WarningToast(object displayContent) : base(displayContent, CuedContentType.Warning) { }

        public WarningToast(object displayContent, CuedContentType cuedContentType, ImageSource iconSource) : base(displayContent, cuedContentType, iconSource) { }
    }
}
