using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class InfoToast : IconToast
    {
        public InfoToast() : base(null, CuedContentType.Info) { }

        public InfoToast(object displayContent) : base(displayContent, CuedContentType.Info) { }

        public InfoToast(object displayContent, CuedContentType cuedContentType, ImageSource iconSource) : base(displayContent, cuedContentType, iconSource) { }
    }
}
