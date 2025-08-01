using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class ErrorToast : IconToast
    {
        public ErrorToast() : base(null, CuedContentType.Error) { }

        public ErrorToast(object displayContent) : base(displayContent, CuedContentType.Error) { }

        public ErrorToast(object displayContent, CuedContentType cuedContentType, ImageSource iconSource) : base(displayContent, cuedContentType, iconSource) { }
    }
}
