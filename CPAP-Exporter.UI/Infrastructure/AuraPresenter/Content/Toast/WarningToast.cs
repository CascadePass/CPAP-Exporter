using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class WarningToast : IconToast
    {
        public WarningToast() : base(null) { }

        public WarningToast(object displayContent) : base(displayContent) { }

        public WarningToast(object displayContent, ImageSource iconSource) : base(displayContent, iconSource) { }
    }
}
