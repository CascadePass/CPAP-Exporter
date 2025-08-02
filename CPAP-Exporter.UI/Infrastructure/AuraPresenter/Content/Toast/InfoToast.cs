using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class InfoToast : IconToast
    {
        public InfoToast() : base(null) { }

        public InfoToast(object displayContent) : base(displayContent) { }

        public InfoToast(object displayContent, ImageSource iconSource) : base(displayContent, iconSource) { }
    }
}
