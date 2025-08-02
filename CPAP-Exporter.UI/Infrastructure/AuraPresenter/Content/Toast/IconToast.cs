using System.Windows.Controls;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class IconToast : Toast
    {
        #region Private Fields

        private ImageSource imageSource;
        private Stretch stretch;
        private double size, margin;
        private Dock dockPosition;

        #endregion

        #region Constructors

        public IconToast() : this(null, null) { }

        public IconToast(object displayContent) : this(displayContent, null) { }


        public IconToast(object displayContent, ImageSource iconSource)
            : base(displayContent)
        {
            this.IconSource = iconSource;
            this.Stretch = Stretch.Uniform;
            this.IconSize = 16;
            this.IconMargin = 4;
            this.IconPlacement = Dock.Left;
        }

        #endregion

        #region Properties

        public ImageSource IconSource {
            get => this.imageSource;
            set => this.SetPropertyValue(ref this.imageSource, value, nameof(this.IconSource));
        }

        public Stretch Stretch {
            get => this.stretch;
            set => this.SetPropertyValue(ref this.stretch, value, nameof(this.Stretch));
        }

        public double IconSize
        {
            get => this.size;
            set => this.SetPropertyValue(ref this.size, value, nameof(this.IconSize));
        }

        public double IconMargin
        {
            get => this.margin;
            set => this.SetPropertyValue(ref this.margin, value, nameof(this.IconMargin));
        }

        public Dock IconPlacement
        {
            get => this.dockPosition;
            set => this.SetPropertyValue(ref this.dockPosition, value, nameof(this.IconPlacement));
        }

        #endregion
    }
}
