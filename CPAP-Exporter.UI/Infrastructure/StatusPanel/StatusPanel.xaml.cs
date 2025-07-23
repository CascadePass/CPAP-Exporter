using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Interaction logic for StatusPanel.xaml
    /// </summary>
    public partial class StatusPanel : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty StatusMessageProperty =
            DependencyProperty.Register(nameof(StatusMessage), typeof(object), typeof(StatusPanel),
                new PropertyMetadata(null, OnStatusMessageChanged));

        public static readonly DependencyProperty MessageBorderThicknessProperty =
            DependencyProperty.Register(nameof(MessageBorderThickness), typeof(Thickness), typeof(StatusPanel),
                new PropertyMetadata(new Thickness(1.5)));

        public static readonly DependencyProperty ShowShadowProperty =
            DependencyProperty.Register(
                nameof(ShowShadow),
                typeof(bool),
                typeof(StatusPanel),
                new PropertyMetadata(true));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(StatusPanel),
                new PropertyMetadata(4.0));

        public static readonly DependencyProperty AttentionStripeWidthProperty =
            DependencyProperty.Register(nameof(AttentionStripeWidth), typeof(double), typeof(StatusPanel),
                new PropertyMetadata(4.0));

        public static readonly DependencyProperty AttentionStripeBrushProperty =
            DependencyProperty.Register(nameof(AttentionStripeBrush), typeof(Brush), typeof(StatusPanel),
                new PropertyMetadata(new SolidColorBrush(Colors.Goldenrod), OnAttentionStripeBrushChanged));

        public static readonly DependencyProperty BackgroundBrushProperty =
            DependencyProperty.Register(nameof(BackgroundBrush), typeof(Brush), typeof(StatusPanel),
                new PropertyMetadata(StatusPanel.DefaultStatusBackgroundBrush));

        public static readonly DependencyProperty PulseStartColorProperty =
            DependencyProperty.Register(nameof(PulseStartColor), typeof(Color), typeof(StatusPanel),
                new PropertyMetadata((Color)ColorConverter.ConvertFromString("#FFC891")));

        public static readonly DependencyProperty PulseEndColorProperty =
            DependencyProperty.Register(nameof(PulseEndColor), typeof(Color), typeof(StatusPanel),
                new PropertyMetadata((Color)ColorConverter.ConvertFromString("#FFD971")));

        #endregion

        public StatusPanel()
        {
            this.InitializeComponent();
        }

        private static readonly LinearGradientBrush DefaultStatusBackgroundBrush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1),
            GradientStops =
            [
                new GradientStop(Color.FromArgb(0xCC, 0xFF, 0xF4, 0xC1), 0.0),
                new GradientStop(Color.FromArgb(0xB0, 0xFF, 0xEB, 0xA3), 1.0)
            ]
        };

        public object StatusMessage
        {
            get => GetValue(StatusMessageProperty);
            set => SetValue(StatusMessageProperty, value);
        }

        public Brush BackgroundBrush
        {
            get => (Brush)GetValue(BackgroundBrushProperty);
            set => SetValue(BackgroundBrushProperty, value);
        }

        public Brush AttentionStripeBrush
        {
            get => (Brush)GetValue(AttentionStripeBrushProperty);
            set => SetValue(AttentionStripeBrushProperty, value);
        }

        public double AttentionStripeWidth
        {
            get => (double)GetValue(AttentionStripeWidthProperty);
            set => SetValue(AttentionStripeWidthProperty, value);
        }

        public bool ShowShadow
        {
            get => (bool)GetValue(ShowShadowProperty);
            set => SetValue(ShowShadowProperty, value);
        }

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Thickness MessageBorderThickness
        {
            get => (Thickness)GetValue(MessageBorderThicknessProperty);
            set => SetValue(MessageBorderThicknessProperty, value);
        }

        public Color PulseStartColor
        {
            get => (Color)GetValue(PulseStartColorProperty);
            set => SetValue(PulseStartColorProperty, value);
        }

        public Color PulseEndColor
        {
            get => (Color)GetValue(PulseEndColorProperty);
            set => SetValue(PulseEndColorProperty, value);
        }

        public void FadeIn()
        {
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            this.BeginAnimation(OpacityProperty, fadeIn);
        }

        public void FadeOut()
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
            this.BeginAnimation(OpacityProperty, fadeOut);
        }

        private void ForceShowMessage()
        {
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(0));
            this.BeginAnimation(OpacityProperty, fadeIn);
        }

        private void PulseBorderColor(Color fromColor, Color toColor, TimeSpan duration)
        {
            if (this.StatusPanelBorder.BorderBrush is SolidColorBrush originalBrush)
            {
                var cloneBrush = originalBrush.Clone();
                this.StatusPanelBorder.BorderBrush = cloneBrush;

                var animation = new ColorAnimation
                {
                    From = fromColor,
                    To = toColor,
                    Duration = duration,
                    AutoReverse = true,
                    FillBehavior = FillBehavior.Stop
                };

                cloneBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
        }

        private void AnimateAttentionStripeColor(StatusPanel panel, SolidColorBrush oldBrush, SolidColorBrush newBrush)
        {
            // Get the Rectangle that uses the brush (named in XAML or found via visual tree if needed)
            if (panel.FindName("AttentionStripe") is Rectangle stripe)
            {
                // If the current brush is frozen (e.g., a StaticResource), clone it so it can be animated
                SolidColorBrush animatedBrush = newBrush?.IsFrozen == true
                    ? newBrush.Clone()
                    : newBrush;

                stripe.Fill = animatedBrush;

                if (oldBrush != null && newBrush != null)
                {
                    var animation = new ColorAnimation
                    {
                        From = oldBrush.Color,
                        To = newBrush.Color,
                        Duration = TimeSpan.FromMilliseconds(300),
                        AutoReverse = false,
                        FillBehavior = FillBehavior.HoldEnd
                    };

                    animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }
        }

        private void DisplayStatusMessage(IStatusMessage message)
        {
            if (message == null)
            {
                this.StatusPanelBorder.Visibility = Visibility.Collapsed;
                return;
            }

            this.StatusPanelBorder.Visibility = Visibility.Visible;

            // Set properties from the message
            this.Foreground = message.ForegroundBrush ?? this.Foreground;
            this.BorderBrush = message.BorderBrush ?? this.BorderBrush;
            this.StatusPanelBorder.BorderBrush = message.BorderBrush ?? this.StatusPanelBorder.BorderBrush;
            this.BackgroundBrush = message.BackgroundBrush ?? this.BackgroundBrush;
            this.AttentionStripeBrush = message.AttentionStripeBrush ?? this.AttentionStripeBrush;
            this.AttentionStripeWidth = message.AttentionStripeWidth ?? this.AttentionStripeWidth;
            this.ShowShadow = message.ShowShadow ?? this.ShowShadow;
            this.CornerRadius = message.CornerRadius ?? this.CornerRadius;
            this.BorderThickness = message.BorderThickness ?? this.BorderThickness;

            // Handle fade in/out and pulse border
            if (message.FadeMessageIn == true)
            {
                this.FadeIn();
            }
            else
            {
                this.ForceShowMessage();
            }

            if (message.PulseBorder == true)
            {
                this.PulseBorderColor(
                    fromColor: message.PulseStartColor ?? this.PulseStartColor,
                    toColor: message.PulseEndColor ?? this.PulseEndColor,
                    duration: TimeSpan.FromMilliseconds(400));
            }
        }

        private static void OnStatusMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not StatusPanel panel)
            {
                return;
            }

            if (e.NewValue is IStatusMessage message)
            {
                panel.DisplayStatusMessage(message);
                return;
            }

            if (e.OldValue == null && e.NewValue != null)
            {
                panel.FadeIn();
            }
            else if (e.OldValue != null && e.NewValue == null)
            {
                panel.FadeOut();
            }

            if (e.NewValue != null)
            {
                // Pulse the border color for attention
                panel.PulseBorderColor(
                    fromColor: panel.PulseStartColor,
                    toColor: panel.PulseEndColor,
                    duration: TimeSpan.FromMilliseconds(400));
            }
        }

        private static void OnAttentionStripeBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not StatusPanel panel)
            {
                return;
            }

            var oldBrush = e.OldValue as SolidColorBrush;
            var newBrush = e.NewValue as SolidColorBrush;

            panel.AnimateAttentionStripeColor(panel, oldBrush, newBrush);
        }
    }

    public interface IStatusMessage
    {
        StatusMessageType MessageType { get; }

        object MessageContent { get; set; }

        #region Panel

        Brush BorderBrush { get; set; }
        Brush ForegroundBrush { get; set; }
        Brush BackgroundBrush { get; set; }
        Brush AttentionStripeBrush { get; set; }

        Color? PulseStartColor { get; set; }
        Color? PulseEndColor { get; set; }
        
        double? AttentionStripeWidth { get; set; }

        bool? ShowShadow { get; set; }

        double? CornerRadius { get; set; }
        Thickness? BorderThickness { get; set; }

        #endregion

        #region Message

        bool? FadeMessageIn { get; set; }
        bool? FadeMessageOut { get; set; }
        bool? PulseBorder { get; set; }

        #endregion
    }

    public class StatusPanelMessage : IStatusMessage
    {
        public StatusPanelMessage() { }

        public StatusPanelMessage(object messageContent) {
            this.MessageContent = messageContent;
        }

        public StatusPanelMessage(object messageContent, StatusMessageType messageType)
        {
            this.MessageContent = messageContent;
            this.MessageType = messageType;
        }

        public StatusMessageType MessageType { get; set; }
        public object MessageContent { get; set; }

        #region Panel Properties

        public Brush BorderBrush { get; set; }
        public Brush ForegroundBrush { get; set; }
        public Brush BackgroundBrush { get; set; }

        public Color? PulseStartColor { get; set; }
        public Color? PulseEndColor { get; set; }
        

        public Brush AttentionStripeBrush { get; set; }
        public double? AttentionStripeWidth { get; set; }

        public bool? ShowShadow { get; set; }
        public double? CornerRadius { get; set; }
        public Thickness? BorderThickness { get; set; }

        #endregion

        #region Animation Control Properties

        public bool? FadeMessageIn { get; set; }
        public bool? FadeMessageOut { get; set; }
        public bool? PulseBorder { get; set; }

        #endregion
    }

    public class InfoStatusMessage : StatusPanelMessage
    {
        public InfoStatusMessage() : base(null, StatusMessageType.Info) { }

        public InfoStatusMessage(object messageContent) : base(messageContent, StatusMessageType.Info) { }
    }

    public class WarningStatusMessage : StatusPanelMessage
    {
        public WarningStatusMessage() : base(null, StatusMessageType.Warning) { }

        public WarningStatusMessage(object messageContent) : base(messageContent, StatusMessageType.Warning) { }
    }
}
