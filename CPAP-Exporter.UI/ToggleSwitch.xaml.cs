using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(bool), typeof(ToggleSwitch),
                new PropertyMetadata(false, OnValueChanged));

        public static readonly DependencyProperty KnobSizeProperty =
            DependencyProperty.Register("KnobSize", typeof(double), typeof(ToggleSwitch),
                new PropertyMetadata(26.0));

        public static readonly DependencyProperty TrackCornerRadiusProperty =
            DependencyProperty.Register("TrackCornerRadius", typeof(double), typeof(ToggleSwitch),
                new PropertyMetadata(16.0));

        private TranslateTransform knobTransform;
        private Ellipse knob;

        public ToggleSwitch()
        {
            this.InitializeComponent();

            this.Loaded += (s, e) =>
            {
                this.knob = this.ToggleSwitchButton.Template.FindName("Knob", this.ToggleSwitchButton) as Ellipse;
                this.knobTransform = this.knob.RenderTransform as TranslateTransform;

                // Set initial value
                VisualStateManager.GoToState(this, this.Value ? "Checked" : "Unchecked", false);
            };
        }

        public double KnobSize
        {
            get => (double)GetValue(KnobSizeProperty);
            set => SetValue(KnobSizeProperty, value);
        }

        public bool Value
        {
            get => (bool)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double TrackCornerRadius
        {
            get => (double)GetValue(TrackCornerRadiusProperty);
            set => SetValue(TrackCornerRadiusProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ToggleSwitch)d;
            bool isChecked = (bool)e.NewValue;
            VisualStateManager.GoToState(control, isChecked ? "Checked" : "Unchecked", true);
        }

        private void ToggleButton_ToggleChanged(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.UpdateKnob(this.Value);
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        private void UpdateKnob(bool isChecked)
        {
            double margin = this.knob.Margin.Left + this.knob.Margin.Right;
            var track = ToggleSwitchButton.Template.FindName("Track", ToggleSwitchButton) as Border;
            double travelDistance = track.ActualWidth - knob.ActualWidth - margin;

            var animation = new DoubleAnimation
            {
                To = isChecked ? travelDistance : 0,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            this.knobTransform.BeginAnimation(TranslateTransform.XProperty, animation);



            if (true)
            {
                var bounce = new DoubleAnimation
                {
                    From = (isChecked ? travelDistance : 0) + 4,
                    To = isChecked ? travelDistance : 0,
                    Duration = TimeSpan.FromMilliseconds(100),
                    EasingFunction = new BounceEase { Bounces = 1, Bounciness = 2, EasingMode = EasingMode.EaseOut }
                };
                knobTransform.BeginAnimation(TranslateTransform.XProperty, bounce);
            }


            // Replace frozen brush with a new one
            track.Background = new SolidColorBrush(Colors.LightGray);

            // Now animate safely
            var colorAnimation = new ColorAnimation
            {
                To = isChecked ? Colors.Blue : Colors.LightGray,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            (track.Background as SolidColorBrush)?.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }
    }
}
