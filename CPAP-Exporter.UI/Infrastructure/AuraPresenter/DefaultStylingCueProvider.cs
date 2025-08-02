using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class DefaultStylingCueProvider : Observable, IStylingCueProvider
    {
        #region Private Fields

        private Dictionary<Type, Brush>
            borderBrushes,
            backgroundBrushes,
            foregroundBrushes,
            attentionStripeBrushes
            ;

        private Dictionary<Type, Color>
            pulseStartColor,
            pulseEndColor
            ;

        private Dictionary<Type, double>
            attentionStripeWidth,
            cornerRadius,
            borderThickness
            ;

        private Dictionary<Type, bool>
            displayShadow,
            animateFadeIn,
            animateFadeOut,
            animateBorderPulse
            ;

        #endregion

        public DefaultStylingCueProvider()
        {
            this.AttentionStripeMaxWidth = 16.0;
            this.CreateDictionaries();
        }

        #region Public Properties

        public double AttentionStripeMaxWidth { get; set; }

        public Dictionary<Type, Brush> BorderBrushes
        {
            get => this.borderBrushes;
            set => this.SetPropertyValue(ref this.borderBrushes, value, nameof(this.BorderBrushes));
        }

        public Dictionary<Type, Brush> BackgroundBrushes
        {
            get => this.backgroundBrushes;
            set => this.SetPropertyValue(ref this.backgroundBrushes, value, nameof(this.BackgroundBrushes));
        }

        public Dictionary<Type, Brush> ForegroundBrushes
        {
            get => this.foregroundBrushes;
            set => this.SetPropertyValue(ref this.foregroundBrushes, value, nameof(this.ForegroundBrushes));
        }

        public Dictionary<Type, Brush> AttentionStripeBrushes
        {
            get => this.attentionStripeBrushes;
            set => this.SetPropertyValue(ref this.attentionStripeBrushes, value, nameof(this.AttentionStripeBrushes));
        }

        #endregion

        internal void CreateDictionaries()
        {
            this.borderBrushes = new()
            {
                [typeof(InfoToast)] = new SolidColorBrush(Color.FromRgb(99, 130, 156)),      // Muted steel blue
                [typeof(WarningToast)] = new SolidColorBrush(Color.FromRgb(198, 138, 0)),    // Goldenrod—attention-grabbing, not shouting
                [typeof(ErrorToast)] = new SolidColorBrush(Color.FromRgb(165, 42, 42)),      // Deep brick red—serious but readable
                [typeof(BusyToast)] = new SolidColorBrush(Color.FromRgb(99, 130, 156)),      // Muted steel blue
            };

            this.backgroundBrushes = new()
            {
                [typeof(InfoToast)] = new SolidColorBrush(Color.FromRgb(230, 245, 255)),   // Soft blue tint
                [typeof(WarningToast)] = new SolidColorBrush(Color.FromRgb(255, 248, 220)), // Pale gold
                [typeof(ErrorToast)] = new SolidColorBrush(Color.FromRgb(255, 235, 238)),   // Gentle red-pink
                [typeof(BusyToast)] = new SolidColorBrush(Color.FromRgb(230, 245, 255)),   // Soft blue tint
            };

            this.foregroundBrushes = new()
            {
                [typeof(InfoToast)] = Brushes.DarkSlateGray,
                [typeof(WarningToast)] = Brushes.DarkGoldenrod,
                [typeof(ErrorToast)] = Brushes.Firebrick,
                [typeof(BusyToast)] = Brushes.Black
            };

            this.attentionStripeBrushes = new()
            {
                [typeof(InfoToast)] = new SolidColorBrush(Color.FromRgb(96, 153, 186)),     // Cool blue stripe for calm focus
                [typeof(WarningToast)] = new SolidColorBrush(Color.FromRgb(255, 175, 0)),    // Vivid amber for urgency
                [typeof(ErrorToast)] = new SolidColorBrush(Color.FromRgb(204, 0, 0)),        // Strong red for critical error
                [typeof(BusyToast)] = new SolidColorBrush(Color.FromRgb(96, 153, 186)),     // Cool blue stripe for calm focus
            };

            this.pulseStartColor = new()
            {
                [typeof(InfoToast)] = Color.FromRgb(99, 130, 156),        // Matches steel blue border
                [typeof(WarningToast)] = Color.FromRgb(198, 138, 0),      // Goldenrod
                [typeof(ErrorToast)] = Color.FromRgb(165, 42, 42),        // Brick red
                [typeof(BusyToast)] = Color.FromRgb(99, 130, 156),        // Matches steel blue border
            };

            this.pulseEndColor = new()
            {
                [typeof(InfoToast)] = Color.FromRgb(135, 170, 200),       // Brighter steel blue
                [typeof(WarningToast)] = Color.FromRgb(255, 191, 60),     // Warm amber
                [typeof(ErrorToast)] = Color.FromRgb(220, 30, 30),        // Vivid red
                [typeof(BusyToast)] = Color.FromRgb(135, 170, 200),       // Brighter steel blue
            };

            this.attentionStripeWidth = new()
            {
                [typeof(InfoToast)] = 2,
                [typeof(WarningToast)] = 4,
                [typeof(ErrorToast)] = 6,
                [typeof(BusyToast)] = 2,
            };

            this.cornerRadius = new()
            {
                [typeof(InfoToast)] = 4,
                [typeof(WarningToast)] = 6,
                [typeof(ErrorToast)] = 8,
                [typeof(BusyToast)] = 4,
            };

            this.borderThickness = new()
            {
                [typeof(InfoToast)] = 1,
                [typeof(WarningToast)] = 2,
                [typeof(ErrorToast)] = 2,
                [typeof(BusyToast)] = 1,
            };

            this.displayShadow = new()
            {
                [typeof(InfoToast)] = false,
                [typeof(WarningToast)] = true,
                [typeof(ErrorToast)] = true,
                [typeof(BusyToast)] = true,
            };

            this.animateFadeIn = new()
            {
                [typeof(InfoToast)] = true,
                [typeof(WarningToast)] = true,
                [typeof(ErrorToast)] = false,
                [typeof(BusyToast)] = true,
            };

            this.animateFadeOut = new()
            {
                [typeof(InfoToast)] = true,
                [typeof(WarningToast)] = true,
                [typeof(ErrorToast)] = false,
                [typeof(BusyToast)] = true,
            };

            this.animateBorderPulse = new()
            {
                [typeof(InfoToast)] = false,
                [typeof(WarningToast)] = true,
                [typeof(ErrorToast)] = true,
                [typeof(BusyToast)] = true,
            };
        }

        #region Brushes

        public virtual Brush GetForegroundBrush(IAuraContent auraContent)
        {
            var brush = this.foregroundBrushes.GetValueOrDefault(auraContent?.GetType());

            if (brush is null)
            {
                return Brushes.Black;
            }

            return brush;
        }

        public virtual Brush GetBackgroundBrush(IAuraContent auraContent)
        {
            var brush = this.backgroundBrushes.GetValueOrDefault(auraContent?.GetType());

            if (brush is null)
            {
                return new SolidColorBrush(Color.FromRgb(230, 245, 255));   // Soft blue tint
            }

            return brush;
        }

        public virtual Brush GetBorderBrush(IAuraContent auraContent)
        {
            var brush = this.borderBrushes.GetValueOrDefault(auraContent?.GetType());

            if (brush is null)
            {
                return Brushes.Gray;
            }

            return brush;
        }

        public virtual Brush GetAttentionStripeBrush(IAuraContent auraContent)
        {
            var brush = this.attentionStripeBrushes.GetValueOrDefault(auraContent?.GetType());

            if (brush is null)
            {
                return Brushes.Transparent;
            }

            return brush;
        }

        public virtual Color GetPulseStartColor(IAuraContent auraContent)
        {
            var color = this.pulseStartColor.GetValueOrDefault(auraContent?.GetType());

            //if (color is null)
            //{
            //    return Colors.Gray;
            //}

            return color;
        }

        public virtual Color GetPulseEndColor(IAuraContent auraContent)
        {
            var color = this.pulseEndColor.GetValueOrDefault(auraContent?.GetType());

            //if (color is null)
            //{
            //    return Colors.Gray;
            //}

            return color;
        }

        #endregion

        #region Control static appearance

        public virtual double GetAttentionStripeWidth(IAuraContent auraContent)
        {
            var width = this.attentionStripeWidth.GetValueOrDefault(auraContent?.GetType());

            double dpiScale = 1.0;

            //// Try to get DPI from the application’s main window
            //if (Application.Current?.MainWindow != null)
            //{
            //    var dpiInfo = VisualTreeHelper.GetDpi(Application.Current.MainWindow);
            //    dpiScale = dpiInfo.DpiScaleX;
            //}

            return Math.Min(width * dpiScale, this.AttentionStripeMaxWidth);
        }

        public virtual double GetCornerRadius(IAuraContent auraContent)
        {
            var radius = this.cornerRadius.GetValueOrDefault(auraContent?.GetType());

            return radius;
        }

        public virtual Thickness GetBorderThickness(IAuraContent auraContent)
        {
            var thicknessValue = this.borderThickness.GetValueOrDefault(auraContent?.GetType());

            return new Thickness(thicknessValue);
        }

        public virtual bool GetShowDropShadow(IAuraContent auraContent)
        {
            var useShadow = this.displayShadow.GetValueOrDefault(auraContent?.GetType());

            return useShadow;
        }

        #endregion

        #region Toggle animations

        public virtual bool GetFadeIn(IAuraContent auraContent)
        {
            var useFadeIn = this.animateFadeIn.GetValueOrDefault(auraContent?.GetType());

            return useFadeIn;
        }

        public virtual bool GetFadeOut(IAuraContent auraContent)
        {
            var useFadeOut = this.animateFadeOut.GetValueOrDefault(auraContent?.GetType());

            return useFadeOut;
        }

        public virtual bool GetPulseBorder(IAuraContent auraContent)
        {
            var useBorderPulse = this.animateBorderPulse.GetValueOrDefault(auraContent?.GetType());

            return useBorderPulse;
        }

        #endregion

        public TimeSpan? GetDisplayDuration(IAuraContent auraContent)
        {
            return null;
        }

        public bool? GetIsHoverPauseEnabled(IAuraContent auraContent)
        {
            return true;
        }

        #region Control shadow properties

        public virtual Color GetShadowColor(IAuraContent auraContent)
        {
            return Colors.Black;
        }

        public virtual double GetShadowOpacity(IAuraContent auraContent)
        {
            return 0.5;
        }

        public virtual double GetShadowBlurRadius(IAuraContent auraContent)
        {
            return 10.0; // Soft shadow
        }

        public virtual double GetShadowDepth(IAuraContent auraContent)
        {
            return 5.0; // Subtle depth
        }

        #endregion
    }
}
