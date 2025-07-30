using System.Windows.Controls;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class IconCueElement : ContentStylingCue
    {
        private ImageSource imageSource;

        public IconCueElement() : this(null, CuedContentType.Custom, null) { }

        public IconCueElement(object messageContent) : this(messageContent, CuedContentType.Custom, null) { }

        public IconCueElement(object messageContent, CuedContentType cuedContentType) : this(messageContent, cuedContentType, null) { }

        public IconCueElement(object messageContent, CuedContentType cuedContentType, ImageSource iconSource)
            : base(messageContent, cuedContentType)
        {
            this.IconSource = iconSource;
            this.Stretch = Stretch.Uniform;
            this.IconSize = 16;
            this.IconMargin = 4;
            this.IconPlacement = Dock.Left;
        }

        public ImageSource IconSource {
            get => this.imageSource;
            set => this.SetPropertyValue(ref this.imageSource, value, nameof(this.IconSource));
        }

        public Stretch Stretch { get; set; }

        public double IconSize { get; set; }

        public double IconMargin { get; set; }

        public Dock IconPlacement { get; set; }
    }
}
