using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class ErrorToast : IconToast
    {
        public ErrorToast() : base(null) { }

        public ErrorToast(object displayContent) : base(displayContent) { }

        public ErrorToast(object displayContent, ImageSource iconSource) : base(displayContent, iconSource) { }
    }
}
