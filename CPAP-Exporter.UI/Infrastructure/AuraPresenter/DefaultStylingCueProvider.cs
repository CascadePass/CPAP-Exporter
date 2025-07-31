using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class DefaultStylingCueProvider : IStylingCueProvider
    {
        public DefaultStylingCueProvider()
        {
            this.AttentionStripeMaxWidth = 16.0;
        }

        public double AttentionStripeMaxWidth { get; set; }

        #region Brushes

        public virtual Brush GetForegroundBrush(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return Brushes.Black;
            }

            return message.ContentType switch
            {
                CuedContentType.Info => Brushes.DarkSlateGray,
                CuedContentType.Warning => Brushes.DarkGoldenrod,
                CuedContentType.Error => Brushes.Firebrick,
                CuedContentType.None => Brushes.Black,
                CuedContentType.Busy => Brushes.Black,
                CuedContentType.Custom => Brushes.Black,
                _ => Brushes.Black,
            };
        }

        public virtual Brush GetBackgroundBrush(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.
                return Brushes.Transparent;
            }

            return message.ContentType switch
            {
                CuedContentType.None => Brushes.Transparent,
                CuedContentType.Info => new SolidColorBrush(Color.FromRgb(230, 245, 255)),   // Soft blue tint
                CuedContentType.Warning => new SolidColorBrush(Color.FromRgb(255, 248, 220)), // Pale gold
                CuedContentType.Error => new SolidColorBrush(Color.FromRgb(255, 235, 238)),   // Gentle red-pink
                CuedContentType.Busy => new SolidColorBrush(Color.FromRgb(230, 245, 255)),   // Soft blue tint
                CuedContentType.Custom => new SolidColorBrush(Color.FromRgb(230, 245, 255)),   // Soft blue tint
                _ => Brushes.Transparent,
            };
        }

        public virtual Brush GetStatusPanelBorderBrush(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.
                return Brushes.Transparent;
            }

            return message.ContentType switch
            {
                CuedContentType.None => Brushes.Gray,                         // Neutral fallback
                CuedContentType.Info => new SolidColorBrush(Color.FromRgb(99, 130, 156)),      // Muted steel blue
                CuedContentType.Warning => new SolidColorBrush(Color.FromRgb(198, 138, 0)),    // Goldenrod—attention-grabbing, not shouting
                CuedContentType.Error => new SolidColorBrush(Color.FromRgb(165, 42, 42)),      // Deep brick red—serious but readable
                CuedContentType.Busy => new SolidColorBrush(Color.FromRgb(99, 130, 156)),      // Muted steel blue
                CuedContentType.Custom => new SolidColorBrush(Color.FromRgb(99, 130, 156)),      // Muted steel blue
                _ => Brushes.Gray,
            };
        }

        public virtual Brush GetAttentionStripeBrush(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage—may be raw UI content.
                return Brushes.Transparent;
            }

            return message.ContentType switch
            {
                CuedContentType.None => Brushes.Transparent,
                CuedContentType.Info => new SolidColorBrush(Color.FromRgb(96, 153, 186)),     // Cool blue stripe for calm focus
                CuedContentType.Warning => new SolidColorBrush(Color.FromRgb(255, 175, 0)),    // Vivid amber for urgency
                CuedContentType.Error => new SolidColorBrush(Color.FromRgb(204, 0, 0)),        // Strong red for critical error
                CuedContentType.Busy => new SolidColorBrush(Color.FromRgb(96, 153, 186)),     // Cool blue stripe for calm focus
                CuedContentType.Custom => new SolidColorBrush(Color.FromRgb(96, 153, 186)),     // Cool blue stripe for calm focus
                _ => Brushes.Transparent,
            };
        }

        public virtual Color GetPulseStartColor(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage—may be raw UI content.

                return Colors.Gray;
            }

            return message.ContentType switch
            {
                CuedContentType.None => Colors.Gray,
                CuedContentType.Info => Color.FromRgb(99, 130, 156),        // Matches steel blue border
                CuedContentType.Warning => Color.FromRgb(198, 138, 0),      // Goldenrod
                CuedContentType.Error => Color.FromRgb(165, 42, 42),        // Brick red
                CuedContentType.Busy => Color.FromRgb(99, 130, 156),        // Matches steel blue border
                CuedContentType.Custom => Color.FromRgb(99, 130, 156),        // Matches steel blue border
                _ => Colors.Gray,
            };
        }

        public virtual Color GetPulseEndColor(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage—may be raw UI content.

                return Colors.DarkGray;
            }


            return message.ContentType switch
            {
                CuedContentType.None => Colors.DarkGray,
                CuedContentType.Info => Color.FromRgb(135, 170, 200),       // Brighter steel blue
                CuedContentType.Warning => Color.FromRgb(255, 191, 60),     // Warm amber
                CuedContentType.Error => Color.FromRgb(220, 30, 30),        // Vivid red
                CuedContentType.Busy => Color.FromRgb(135, 170, 200),       // Brighter steel blue
                CuedContentType.Custom => Color.FromRgb(135, 170, 200),       // Brighter steel blue
                _ => Colors.DarkGray,
            };
        }

        #endregion

        #region Control static appearance

        public virtual double GetAttentionStripeWidth(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return 0;
            }

            double dpiScale = 1.0;

            // Try to get DPI from the application’s main window
            if (Application.Current?.MainWindow != null)
            {
                var dpiInfo = VisualTreeHelper.GetDpi(Application.Current.MainWindow);
                dpiScale = dpiInfo.DpiScaleX;
            }

            double baseWidth = message.ContentType switch
            {
                CuedContentType.None => 0,
                CuedContentType.Info => 2,
                CuedContentType.Warning => 4,
                CuedContentType.Error => 6,
                CuedContentType.Busy => 2,
                CuedContentType.Custom => 2,
                _ => 0,
            };

            return Math.Min(baseWidth * dpiScale, this.AttentionStripeMaxWidth);
        }

        public virtual double GetCornerRadius(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return 0;
            }

            return message.ContentType switch
            {
                CuedContentType.None => 0,
                CuedContentType.Info => 4,
                CuedContentType.Warning => 6,
                CuedContentType.Error => 8,
                CuedContentType.Busy => 4,
                CuedContentType.Custom => 4,
                _ => 0,
            };
        }

        public virtual Thickness GetBorderThickness(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return new Thickness(1);
            }

            return message.ContentType switch
            {
                CuedContentType.None => new Thickness(1),
                CuedContentType.Info => new Thickness(1),
                CuedContentType.Warning => new Thickness(2),
                CuedContentType.Error => new Thickness(2),
                CuedContentType.Busy => new Thickness(1),
                CuedContentType.Custom => new Thickness(1),
                _ => new Thickness(1),
            };
        }

        public virtual bool GetShowDropShadow(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return false;
            }

            return message.ContentType switch
            {
                CuedContentType.None => false,
                CuedContentType.Info => false,
                CuedContentType.Warning => true,
                CuedContentType.Error => true,
                CuedContentType.Busy => true,
                CuedContentType.Custom => false,
                _ => false,
            };
        }

        #endregion

        #region Toggle animations

        public virtual bool GetFadeIn(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return false;
            }

            return message.ContentType switch
            {
                CuedContentType.None => false,         // Ambient or placeholder
                CuedContentType.Info => true,          // Calm, informative tone
                CuedContentType.Warning => true,       // Gives it presence without alarm
                CuedContentType.Error => false,        // Immediate visibility—no delay
                CuedContentType.Busy => true,          // Calm, informative tone
                CuedContentType.Custom => true,          // Calm, informative tone
                _ => false,
            };
        }

        public virtual bool GetFadeOut(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return false;
            }

            return message.ContentType switch
            {
                CuedContentType.None => false,
                CuedContentType.Info => true,
                CuedContentType.Warning => true,
                CuedContentType.Error => false,
                CuedContentType.Busy => true,
                CuedContentType.Custom => true,
                _ => false,
            };
        }

        public virtual bool GetPulseBorder(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return false;
            }

            return message.ContentType switch
            {
                CuedContentType.None => false,
                CuedContentType.Info => false,
                CuedContentType.Warning => true,    // Subtle urgency
                CuedContentType.Error => true,      // Strong emphasis
                CuedContentType.Busy => true,
                CuedContentType.Custom => false,
                _ => false,
            };
        }

        #endregion

        public TimeSpan? GetDisplayDuration(IStylingCue message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return null;
            }

            return message.ContentType switch
            {
                CuedContentType.None => null,
                CuedContentType.Info => null,
                CuedContentType.Warning => null,
                CuedContentType.Error => null,
                CuedContentType.Busy => null,
                CuedContentType.Custom => null,
                _ => null,
            };
        }

        #region Control shadow properties

        public virtual Color GetShadowColor(IStylingCue message)
        {
            return Colors.Black;
        }

        public virtual double GetShadowOpacity(IStylingCue message)
        {
            return 0.5;
        }

        public virtual double GetShadowBlurRadius(IStylingCue message)
        {
            return 10.0; // Soft shadow
        }

        public virtual double GetShadowDepth(IStylingCue message)
        {
            return 5.0; // Subtle depth
        }

        #endregion
    }
}
