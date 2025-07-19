using System.Runtime.CompilerServices;
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
        #region Dependency Properties

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(bool), typeof(ToggleSwitch),
                new PropertyMetadata(false, OnValueChanged));

        #region Control Appearance

        public static readonly DependencyProperty KnobSizeProperty =
            DependencyProperty.Register("KnobSize", typeof(double), typeof(ToggleSwitch),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty TrackCornerRadiusProperty =
            DependencyProperty.Register("TrackCornerRadius", typeof(double), typeof(ToggleSwitch),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty TrackBorderThicknessProperty =
            DependencyProperty.Register("TrackBorderThickness", typeof(double), typeof(ToggleSwitch),
                new PropertyMetadata(1.0));

        public static readonly DependencyProperty KnobMarginProperty =
            DependencyProperty.Register("KnobMargin", typeof(double), typeof(ToggleSwitch),
                new PropertyMetadata(2.0));

        #endregion

        #region Animation

        public static readonly DependencyProperty IsSlideEnabledProperty =
            DependencyProperty.Register("IsSlideEnabled", typeof(bool), typeof(ToggleSwitch),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsBounceEnabledProperty =
            DependencyProperty.Register("IsBounceEnabled", typeof(bool), typeof(ToggleSwitch),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsFadeEnabledProperty =
            DependencyProperty.Register("IsFadeEnabled", typeof(bool), typeof(ToggleSwitch),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsBackgroundAnimationEnabledProperty =
            DependencyProperty.Register("IsBackgroundAnimationEnabled", typeof(bool), typeof(ToggleSwitch),
                new PropertyMetadata(true));

        #endregion

        #region Brushes

        public static readonly DependencyProperty KnobForegroundBrushProperty =
            DependencyProperty.Register(nameof(KnobForegroundBrush), typeof(Brush), typeof(ToggleSwitch),
                new PropertyMetadata(Brushes.White, null));

        public static readonly DependencyProperty KnobUnselectedForegroundBrushProperty =
            DependencyProperty.Register(nameof(KnobUnselectedForegroundBrush), typeof(Brush), typeof(ToggleSwitch),
                new PropertyMetadata(Brushes.DarkGray, null));

        public static readonly DependencyProperty TrackBorderBrushProperty =
            DependencyProperty.Register(nameof(TrackBorderBrush), typeof(Brush), typeof(ToggleSwitch),
                new PropertyMetadata(Brushes.Gray, null));

        public static readonly DependencyProperty TrackBackgroundBrushProperty =
            DependencyProperty.Register(nameof(TrackBackgroundBrush), typeof(Brush), typeof(ToggleSwitch),
                new PropertyMetadata(Brushes.LightSlateGray, null));

        public static readonly DependencyProperty TrackCheckedBackgroundBrushProperty =
            DependencyProperty.Register(nameof(TrackCheckedBackgroundBrush), typeof(Brush), typeof(ToggleSwitch),
                new PropertyMetadata(Brushes.Blue, null));

        public static readonly DependencyProperty TrackDisabledBackgroundBrushProperty =
            DependencyProperty.Register(nameof(TrackDisabledBackgroundBrush), typeof(Brush), typeof(ToggleSwitch),
                new PropertyMetadata(Brushes.LightGray, null));

        public static readonly DependencyProperty KnobDisabledBackgroundBrushProperty =
            DependencyProperty.Register(nameof(KnobDisabledBackgroundBrush), typeof(Brush), typeof(ToggleSwitch),
                new PropertyMetadata(Brushes.Gray, null));

        #endregion

        #endregion

        #region Fields

        private bool autoSizeKnob, autoRoundCorners;
        private TranslateTransform knobTransform;
        private Ellipse knob;
        private Border track;

        #endregion

        #region Constructor

        public ToggleSwitch()
        {
            this.InitializeComponent();

            this.Loaded += this.ToggleSwitch_Loaded;
        }

        #endregion

        #region Properties

        public bool Value
        {
            get => (bool)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        #region Behavior Appearance

        public double KnobSize
        {
            get => (double)GetValue(KnobSizeProperty);
            set => SetValue(KnobSizeProperty, value);
        }

        public double TrackCornerRadius
        {
            get => (double)GetValue(TrackCornerRadiusProperty);
            set => SetValue(TrackCornerRadiusProperty, value);
        }

        public double TrackBorderThickness
        {
            get => (double)GetValue(TrackBorderThicknessProperty);
            set => SetValue(TrackBorderThicknessProperty, value);
        }

        public double KnobMargin
        {
            get => (double)GetValue(KnobMarginProperty);
            set => SetValue(KnobMarginProperty, value);
        }

        #endregion

        #region Animation

        public bool IsSlideEnabled
        {
            get => (bool)GetValue(IsSlideEnabledProperty);
            set => SetValue(IsSlideEnabledProperty, value);
        }

        public bool IsBounceEnabled
        {
            get => (bool)GetValue(IsBounceEnabledProperty);
            set => SetValue(IsBounceEnabledProperty, value);
        }

        public bool IsFadeEnabled
        {
            get => (bool)GetValue(IsFadeEnabledProperty);
            set => SetValue(IsFadeEnabledProperty, value);
        }

        public bool IsBackgroundAnimationEnabled
        {
            get => (bool)GetValue(IsBackgroundAnimationEnabledProperty);
            set => SetValue(IsBackgroundAnimationEnabledProperty, value);
        }

        #endregion

        #region Brushes

        public Brush KnobForegroundBrush
        {
            get => (Brush)GetValue(KnobForegroundBrushProperty);
            set => SetValue(KnobForegroundBrushProperty, value);
        }

        public Brush KnobUnselectedForegroundBrush
        {
            get => (Brush)GetValue(KnobUnselectedForegroundBrushProperty);
            set => SetValue(KnobUnselectedForegroundBrushProperty, value);
        }

        public Brush TrackBorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public Brush TrackBackgroundBrush
        {
            get => (Brush)GetValue(TrackBackgroundBrushProperty);
            set => SetValue(TrackBackgroundBrushProperty, value);
        }

        public Brush TrackCheckedBackgroundBrush
        {
            get => (Brush)GetValue(TrackCheckedBackgroundBrushProperty);
            set => SetValue(TrackCheckedBackgroundBrushProperty, value);
        }

        public Brush TrackDisabledBackgroundBrush
        {
            get => (Brush)GetValue(TrackDisabledBackgroundBrushProperty);
            set => SetValue(TrackDisabledBackgroundBrushProperty, value);
        }

        public Brush KnobDisabledBackgroundBrush
        {
            get => (Brush)GetValue(KnobDisabledBackgroundBrushProperty);
            set => SetValue(KnobDisabledBackgroundBrushProperty, value);
        }

        #endregion

        #endregion

        #region Methods

        internal string GetStateName(bool isChecked) => isChecked ? "Checked" : "Unchecked";

        private void AnimateKnob(bool isChecked)
        {
            double margin = this.knob.Margin.Left + this.knob.Margin.Right;
            double travelDistance = this.track.ActualWidth - this.knob.ActualWidth - margin;

            if (this.IsSlideEnabled)
            {
                var animation = new DoubleAnimation
                {
                    To = isChecked ? travelDistance : 0,
                    Duration = TimeSpan.FromMilliseconds(250),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                this.knobTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            }


            if (this.IsBounceEnabled)
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


            if (this.IsBackgroundAnimationEnabled)
            {
                if (track?.Background is SolidColorBrush brush)
                {
                    var colorAnimation = new ColorAnimation
                    {
                        To = isChecked ? ((SolidColorBrush)this.TrackCheckedBackgroundBrush).Color : ((SolidColorBrush)this.TrackBackgroundBrush).Color,
                        Duration = TimeSpan.FromMilliseconds(250),
                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                    };

                    brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                }
            }


            if (this.IsFadeEnabled)
            {
                var fade = new DoubleAnimation
                {
                    To = isChecked ? 1.0 : 0.6,
                    Duration = TimeSpan.FromMilliseconds(250),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                ToggleSwitchButton.BeginAnimation(UIElement.OpacityProperty, fade);
            }
        }

        internal void AutoSizeKnob()
        {
            if (!this.autoSizeKnob)
            {
                return;
            }

            var margin = this.knob.Margin.Top + this.knob.Margin.Bottom;
            this.KnobSize = this.ActualHeight - margin;
        }

        internal void AutoRoundBorderCorners()
        {
            if (!this.autoRoundCorners)
            {
                return;
            }

            this.TrackCornerRadius = Math.Min(this.ActualHeight, this.ActualWidth) / 2;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ToggleSwitch)d;
            bool isChecked = (bool)e.NewValue;

            if (control?.knob is not null)
            {
                control.knob.Fill = isChecked ? control.KnobForegroundBrush : control.KnobUnselectedForegroundBrush;
            }

            VisualStateManager.GoToState(control, control.GetStateName(isChecked), true);
        }

        private void ToggleButton_ToggleChanged(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.AnimateKnob(this.Value);
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        private void ToggleSwitch_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.AutoSizeKnob();
            this.AutoRoundBorderCorners();
        }

        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            // Acquire references to needed parts of the control
            this.knob = this.ToggleSwitchButton.Template.FindName("Knob", this.ToggleSwitchButton) as Ellipse;
            this.track = this.ToggleSwitchButton.Template.FindName("Track", ToggleSwitchButton) as Border;
            this.knobTransform = this.knob.RenderTransform as TranslateTransform;


            // I need to clone the brush because the original is frozen.
            this.track.Background = this.TrackBackgroundBrush.Clone();


            // Save values telling me whether to resize things
            this.autoSizeKnob = this.KnobSize == 0;
            this.autoRoundCorners = this.TrackCornerRadius == 0;


            // Now act on sizing, if necessary
            if (this.autoSizeKnob || this.autoRoundCorners)
            {
                this.SizeChanged += this.ToggleSwitch_SizeChanged;

                // The event won't be called at this point, do it manually:
                this.AutoSizeKnob();
                this.AutoRoundBorderCorners();
            }

            // Set initial value
            VisualStateManager.GoToState(this, this.GetStateName(this.Value), false);
            this.AnimateKnob(this.Value);
        }

        #endregion
    }
}
