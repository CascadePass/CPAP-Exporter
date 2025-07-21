using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Interaction logic for StatusPanel.xaml
    /// </summary>
    public partial class StatusPanel : UserControl
    {
        public static readonly DependencyProperty StatusMessageProperty =
            DependencyProperty.Register("StatusMessage", typeof(object), typeof(StatusPanel),
                new PropertyMetadata(null, OnStatusMessageChanged));

        public static readonly DependencyProperty StatusBackgroundBrushProperty =
            DependencyProperty.Register("StatusBackgroundBrush", typeof(Brush), typeof(StatusPanel),
                new PropertyMetadata(StatusPanel.DefaultStatusBackgroundBrush));

        public static readonly DependencyProperty ShowShadowProperty =
            DependencyProperty.Register(
                nameof(ShowShadow),
                typeof(bool),
                typeof(StatusPanel),
                new PropertyMetadata(true));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(StatusPanel),
                new PropertyMetadata(4.0));

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

        public Brush StatusBackgroundBrush
        {
            get => (Brush)GetValue(StatusBackgroundBrushProperty);
            set => SetValue(StatusBackgroundBrushProperty, value);
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

        private static void OnStatusMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StatusPanel panel)
            {
                if (e.OldValue == null && e.NewValue != null)
                {
                    panel.FadeIn();
                }
                else if(e.OldValue != null && e.NewValue == null)
                {
                    panel.FadeOut();
                }

                if (e.NewValue != null)
                {
                    // Pulse the border color for attention
                    panel.PulseBorderColor(
                        fromColor: (Color)ColorConverter.ConvertFromString("#FFC891"),
                        toColor: (Color)ColorConverter.ConvertFromString("#FFD971"),
                        duration: TimeSpan.FromMilliseconds(400));
                }
            }
        }
    }
}
