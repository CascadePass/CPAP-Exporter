using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public class DefaultStatusMessageStyleProvider : IStatusMessageStyleProvider
    {
        #region Brushes

        public virtual Brush GetForegroundBrush(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return Brushes.Black;
            }

            return message.MessageType switch
            {
                StatusMessageType.Info => Brushes.DarkSlateGray,
                StatusMessageType.Warning => Brushes.DarkGoldenrod,
                StatusMessageType.Error => Brushes.Firebrick,
                StatusMessageType.None => Brushes.Black,
                _ => Brushes.Black,
            };
        }

        public virtual Brush GetBackgroundBrush(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.
                return Brushes.Transparent;
            }

            return message.MessageType switch
            {
                StatusMessageType.None => Brushes.Transparent,
                StatusMessageType.Info => new SolidColorBrush(Color.FromRgb(230, 245, 255)),   // Soft blue tint
                StatusMessageType.Warning => new SolidColorBrush(Color.FromRgb(255, 248, 220)), // Pale gold
                StatusMessageType.Error => new SolidColorBrush(Color.FromRgb(255, 235, 238)),   // Gentle red-pink
                _ => Brushes.Transparent,
            };
        }

        public virtual Brush GetStatusPanelBorderBrush(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.
                return Brushes.Transparent;
            }

            return message.MessageType switch
            {
                StatusMessageType.None => Brushes.Gray,                         // Neutral fallback
                StatusMessageType.Info => new SolidColorBrush(Color.FromRgb(99, 130, 156)),      // Muted steel blue
                StatusMessageType.Warning => new SolidColorBrush(Color.FromRgb(198, 138, 0)),    // Goldenrod—attention-grabbing, not shouting
                StatusMessageType.Error => new SolidColorBrush(Color.FromRgb(165, 42, 42)),      // Deep brick red—serious but readable
                _ => Brushes.Gray,
            };
        }

        public virtual Brush GetAttentionStripeBrush(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage—may be raw UI content.
                return Brushes.Transparent;
            }

            return message.MessageType switch
            {
                StatusMessageType.None => Brushes.Transparent,
                StatusMessageType.Info => new SolidColorBrush(Color.FromRgb(96, 153, 186)),     // Cool blue stripe for calm focus
                StatusMessageType.Warning => new SolidColorBrush(Color.FromRgb(255, 175, 0)),    // Vivid amber for urgency
                StatusMessageType.Error => new SolidColorBrush(Color.FromRgb(204, 0, 0)),        // Strong red for critical error
                _ => Brushes.Transparent,
            };
        }

        public virtual Color GetPulseStartColor(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage—may be raw UI content.

                return Colors.Gray;
            }

            return message.MessageType switch
            {
                StatusMessageType.None => Colors.Gray,
                StatusMessageType.Info => Color.FromRgb(99, 130, 156),        // Matches steel blue border
                StatusMessageType.Warning => Color.FromRgb(198, 138, 0),      // Goldenrod
                StatusMessageType.Error => Color.FromRgb(165, 42, 42),        // Brick red
                _ => Colors.Gray,
            };
        }

        public virtual Color GetPulseEndColor(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage—may be raw UI content.

                return Colors.DarkGray;
            }


            return message.MessageType switch
            {
                StatusMessageType.None => Colors.DarkGray,
                StatusMessageType.Info => Color.FromRgb(135, 170, 200),       // Brighter steel blue
                StatusMessageType.Warning => Color.FromRgb(255, 191, 60),     // Warm amber
                StatusMessageType.Error => Color.FromRgb(220, 30, 30),        // Vivid red
                _ => Colors.DarkGray,
            };
        }

        #endregion

        #region Control static appearance

        public virtual double GetAttentionStripeWidth(IStatusMessage message)
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

            double baseWidth = message.MessageType switch
            {
                StatusMessageType.None => 0,
                StatusMessageType.Info => 2,
                StatusMessageType.Warning => 4,
                StatusMessageType.Error => 6,
                _ => 0,
            };

            return Math.Min(baseWidth * dpiScale, 10.0);
        }

        public virtual double GetCornerRadius(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return 0;
            }

            return message.MessageType switch
            {
                StatusMessageType.None => 0,
                StatusMessageType.Info => 4,
                StatusMessageType.Warning => 6,
                StatusMessageType.Error => 8,
                _ => 0,
            };
        }

        public virtual Thickness GetBorderThickness(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return new Thickness(1);
            }

            return message.MessageType switch
            {
                StatusMessageType.None => new Thickness(1),
                StatusMessageType.Info => new Thickness(1),
                StatusMessageType.Warning => new Thickness(2),
                StatusMessageType.Error => new Thickness(2),
                _ => new Thickness(1),
            };
        }

        public virtual bool GetShowDropShadow(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return false;
            }

            return message.MessageType switch
            {
                StatusMessageType.None => false,
                StatusMessageType.Info => false,
                StatusMessageType.Warning => true,
                StatusMessageType.Error => true,
                _ => false,
            };
        }

        #endregion

        #region Toggle animations

        public virtual bool GetFadeIn(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return false;
            }

            return message.MessageType switch
            {
                StatusMessageType.None => false,         // Ambient or placeholder
                StatusMessageType.Info => true,          // Calm, informative tone
                StatusMessageType.Warning => true,       // Gives it presence without alarm
                StatusMessageType.Error => false,        // Immediate visibility—no delay
                _ => false,
            };
        }

        public virtual bool GetFadeOut(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return false;
            }

            return message.MessageType switch
            {
                StatusMessageType.None => false,
                StatusMessageType.Info => true,
                StatusMessageType.Warning => true,
                StatusMessageType.Error => false,
                _ => false,
            };
        }

        public virtual bool GetPulseBorder(IStatusMessage message)
        {
            if (message is null)
            {
                // Fallback:
                // The original message does not implement IStatusMessage, maybe it's a string or a control.

                return false;
            }

            return message.MessageType switch
            {
                StatusMessageType.None => false,
                StatusMessageType.Info => false,
                StatusMessageType.Warning => true,    // Subtle urgency
                StatusMessageType.Error => true,      // Strong emphasis
                _ => false,
            };
        }

        #endregion
    }

    public class CpapExporterStatusMessageStyleProvider : DefaultStatusMessageStyleProvider
    {
        // This class can be used to override any specific styles or behaviors for CPAP Exporter status messages.

        public override Brush GetStatusPanelBorderBrush(IStatusMessage message)
        {
            return message?.MessageType switch
            {
                StatusMessageType.Info => Brushes.DarkBlue, // Custom color for info messages
                StatusMessageType.Warning => Brushes.DarkOrange, // Custom color for warning messages
                StatusMessageType.Error => Brushes.DarkRed, // Custom color for error messages
                _ => base.GetStatusPanelBorderBrush(message), // Fallback to base implementation
            };
        }

        public override Brush GetBackgroundBrush(IStatusMessage message)
        {
            return this.GetStatusPanelBorderBrush(message);
        }
    }
}
