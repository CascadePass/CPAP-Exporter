using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class CpapExporterStylingCueProvider : DefaultStylingCueProvider
    {
        private Dictionary<Type, Brush>
            borderBrushes,
            backgroundBrushes,
            foregroundBrushes,
            attentionStripeBrushes
            ;

        public CpapExporterStylingCueProvider()
        {
            this.CreateDictionaries();
        }

        internal void CreateDictionaries()
        {
            this.borderBrushes = new()
            {
                [typeof(InfoToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.ContentBorderBrush"),
                [typeof(WarningToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.ContentBorderBrush"),
                [typeof(ErrorToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.ContentBorderBrush"),
                [typeof(BusyToast)] = ResourceLocator.GetResource<Brush>("ControlElevationBorderBrush")
            };

            this.backgroundBrushes = new()
            {
                [typeof(InfoToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.Background"),
                [typeof(WarningToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.Background"),
                [typeof(ErrorToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.Background"),
                [typeof(BusyToast)] = ResourceLocator.GetResource<Brush>("Legibility.Background")
            };

            this.foregroundBrushes = new()
            {
                [typeof(InfoToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.Foreground"),
                [typeof(WarningToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.Foreground"),
                [typeof(ErrorToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.Foreground"),
                [typeof(BusyToast)] = Brushes.Transparent
            };

            this.attentionStripeBrushes = new()
            {
                [typeof(InfoToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.AttentionStripeBrush"),
                [typeof(WarningToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.AttentionStripeBrush"),
                [typeof(ErrorToast)] = ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.AttentionStripeBrush"),
                [typeof(BusyToast)] = Brushes.Transparent
            };
        }

        public override Brush GetBorderBrush(IAuraContent auraContent)
        {
            //if(auraContent is InfoToast)
            //{
            //    return ResourceLocator.GetResource<Brush>("StatusPanel.InfoMessage.ContentBorderBrush");
            //}

            //if (auraContent is WarningToast)
            //{
            //    return ResourceLocator.GetResource<Brush>("StatusPanel.WarningMessage.ContentBorderBrush");
            //}

            //if (auraContent is ErrorToast)
            //{
            //    return ResourceLocator.GetResource<Brush>("StatusPanel.ErrorMessage.ContentBorderBrush");
            //}

            //if (auraContent is BusyToast)
            //{
            //    return ResourceLocator.GetResource<Brush>("ControlElevationBorderBrush");
            //}

            var brush = this.borderBrushes.GetValueOrDefault(auraContent?.GetType());

            if (brush is null)
            {
                // Fallback to base implementation
                return base.GetBorderBrush(auraContent);
            }

            return brush;
        }

        public override Brush GetBackgroundBrush(IAuraContent auraContent)
        {
            var brush = this.backgroundBrushes.GetValueOrDefault(auraContent?.GetType());

            if (brush is null)
            {
                // Fallback to base implementation
                return base.GetBackgroundBrush(auraContent);
            }

            return brush;
        }

        public override Brush GetForegroundBrush(IAuraContent auraContent)
        {
            var brush = this.foregroundBrushes.GetValueOrDefault(auraContent?.GetType());

            if (brush is null)
            {
                // Fallback to base implementation
                return base.GetForegroundBrush(auraContent);
            }

            return brush;
        }

        public override Brush GetAttentionStripeBrush(IAuraContent auraContent)
        {
            var brush = this.attentionStripeBrushes.GetValueOrDefault(auraContent?.GetType());

            if (brush is null)
            {
                // Fallback to base implementation
                return base.GetAttentionStripeBrush(auraContent);
            }

            return brush;
        }

        public override double GetAttentionStripeWidth(IAuraContent auraContent)
        {
            return 6;
        }

        public override Color GetShadowColor(IAuraContent auraContent)
        {
            return (Color)ResourceLocator.GetColorResource("StatusPanel.Shadow.Color");
        }

        public override Thickness GetBorderThickness(IAuraContent auraContent)
        {
            return new Thickness(2);
        }
    }
}
