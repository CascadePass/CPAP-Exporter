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
        private readonly HashSet<DependencyProperty> xamlSetProperties;

        #region Dependency Properties

        public static readonly DependencyProperty StatusMessageProperty =
            DependencyProperty.Register(nameof(StatusMessage), typeof(object), typeof(StatusPanel),
                new PropertyMetadata(null, OnStatusMessageChanged));

        public static readonly DependencyProperty MessageBorderThicknessProperty =
            DependencyProperty.Register(nameof(MessageBorderThickness), typeof(Thickness), typeof(StatusPanel),
                new PropertyMetadata(new Thickness(1.5)));

        public static readonly DependencyProperty MessageBorderBrushProperty =
            DependencyProperty.Register(nameof(MessageBorderBrush), typeof(Brush), typeof(StatusPanel),
                new PropertyMetadata(Brushes.Gold));

        public static readonly DependencyProperty ShowDropShadowProperty =
            DependencyProperty.Register(
                nameof(ShowDropShadow),
                typeof(bool),
                typeof(StatusPanel),
                new PropertyMetadata(true));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(StatusPanel),
                new PropertyMetadata(4.0));

        public static readonly DependencyProperty AttentionStripeWidthProperty =
            DependencyProperty.Register(nameof(AttentionStripeWidth), typeof(double), typeof(StatusPanel),
                new FrameworkPropertyMetadata(4.0, FrameworkPropertyMetadataOptions.AffectsRender));

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

        public static readonly DependencyProperty StatusMessageStyleProviderProperty =
            DependencyProperty.Register(nameof(StatusMessageStyleProvider), typeof(IStatusMessageStyleProvider), typeof(StatusPanel),
                new PropertyMetadata(new DefaultStatusMessageStyleProvider()));

        public static readonly DependencyProperty OverrideBehaviorProperty =
            DependencyProperty.Register(
                nameof(OverrideBehavior),
                typeof(MessageOverrideBehavior),
                typeof(StatusPanel),
                new PropertyMetadata(MessageOverrideBehavior.PreferLocalValues)
            );

        #endregion

        public StatusPanel()
        {
            this.xamlSetProperties = [];
            this.InitializeComponent();
        }

        #region Properties

        private static readonly LinearGradientBrush DefaultStatusBackgroundBrush = new()
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1),
            GradientStops =
            [
                new GradientStop(Color.FromArgb(0xCC, 0xFF, 0xF4, 0xC1), 0.0),
                new GradientStop(Color.FromArgb(0xB0, 0xFF, 0xEB, 0xA3), 1.0)
            ]
        };

        #region Dependency Properties

        public object StatusMessage
        {
            get => GetValue(StatusMessageProperty);
            set => SetValue(StatusMessageProperty, value);
        }

        public Brush MessageBorderBrush
        {
            get => (Brush)GetValue(MessageBorderBrushProperty);
            set => SetValue(MessageBorderBrushProperty, value);
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

        public bool ShowDropShadow
        {
            get => (bool)GetValue(ShowDropShadowProperty);
            set => SetValue(ShowDropShadowProperty, value);
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

        public IStatusMessageStyleProvider StatusMessageStyleProvider
        {
            get => (IStatusMessageStyleProvider)GetValue(StatusMessageStyleProviderProperty);
            set => SetValue(StatusMessageStyleProviderProperty, value);
        }

        public MessageOverrideBehavior OverrideBehavior
        {
            get => (MessageOverrideBehavior)GetValue(OverrideBehaviorProperty);
            set => SetValue(OverrideBehaviorProperty, value);
        }

        #endregion

        #endregion

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
                return;
            }

            // Set properties from the message
            this.SetVisualProperties(message);

            // Handle fade in/out and pulse border
            if (!message.FadeMessageIn.HasValue || !message.FadeMessageIn.Value)
            {
                this.ForceShowMessage();
            }

            if (message.PulseBorder == true)
            {
                this.PulseBorderColor(this.PulseStartColor, this.PulseEndColor, TimeSpan.FromMilliseconds(400));
            }
        }

        private void SetVisualProperties(IStatusMessage message)
        {
            this.Foreground = this.ResolveValue(
                StatusPanel.ForegroundProperty,
                message?.ForegroundBrush,
                () => this.StatusMessageStyleProvider.GetForegroundBrush(message)
            );

            this.BackgroundBrush = this.ResolveValue(
                StatusPanel.BackgroundBrushProperty,
                message?.BackgroundBrush,
                () => this.StatusMessageStyleProvider.GetBackgroundBrush(message)
            );

            this.MessageBorderBrush = this.ResolveValue(
                StatusPanel.MessageBorderBrushProperty,
                message?.BorderBrush,
                () => this.StatusMessageStyleProvider.GetStatusPanelBorderBrush(message)
            );

            this.AttentionStripeBrush = this.ResolveValue(
                StatusPanel.AttentionStripeBrushProperty,
                message?.AttentionStripeBrush,
                () => this.StatusMessageStyleProvider.GetAttentionStripeBrush(message)
            );

            this.AttentionStripeWidth = (double)this.ResolveValue(
                StatusPanel.AttentionStripeWidthProperty,
                message?.AttentionStripeWidth,
                () => this.StatusMessageStyleProvider.GetAttentionStripeWidth(message)
            );

            this.ShowDropShadow = (bool)this.ResolveValue(
                StatusPanel.ShowDropShadowProperty,
                message?.ShowDropShadow,
                () => this.StatusMessageStyleProvider.GetShowDropShadow(message)
            );

            this.CornerRadius = (double)this.ResolveValue(
                StatusPanel.CornerRadiusProperty,
                message?.CornerRadius,
                () => this.StatusMessageStyleProvider.GetCornerRadius(message)
            );

            this.BorderThickness = (Thickness)this.ResolveValue(
                StatusPanel.BorderThicknessProperty,
                message?.BorderThickness,
                () => this.StatusMessageStyleProvider.GetBorderThickness(message)
            );
        }

        private T ResolveValue<T>(DependencyProperty property, T messageValue, Func<T> styleProviderValue)
        {
            bool hasLocalValue = this.xamlSetProperties.Contains(property);

            return this.OverrideBehavior switch
            {
                MessageOverrideBehavior.PreferLocalValues => hasLocalValue
                                        ? (T)GetValue(property)
                                        : messageValue ?? styleProviderValue(),

                MessageOverrideBehavior.PreferMessageValues => messageValue ?? (hasLocalValue ? (T)GetValue(property) : styleProviderValue()),

                _ => hasLocalValue
                                        ? (T)GetValue(property)
                                        : messageValue ?? styleProviderValue(),
            };
        }

        private void DiscoverXamlProperties()
        {
            var properties = new[]
            {
                StatusPanel.MessageBorderBrushProperty,
                StatusPanel.PulseStartColorProperty,
                StatusPanel.PulseEndColorProperty,
                StatusPanel.BackgroundBrushProperty,
                StatusPanel.AttentionStripeBrushProperty,
                StatusPanel.AttentionStripeWidthProperty,
                StatusPanel.ShowDropShadowProperty,
                StatusPanel.CornerRadiusProperty,
                StatusPanel.MessageBorderThicknessProperty
            };

            foreach (var prop in properties)
            {
                if (this.ReadLocalValue(prop) != DependencyProperty.UnsetValue)
                {
                    this.xamlSetProperties.Add(prop);
                }
            }
        }

        private void ScaleAttentionStripeForDpi()
        {
            bool hasLocalValue = this.xamlSetProperties.Contains(AttentionStripeWidthProperty);

            if (!hasLocalValue)
            {
                var dpi = VisualTreeHelper.GetDpi(this);
                this.AttentionStripeWidth = dpi.DpiScaleX >= 2.0 ? 6.0 : 4.0;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.DiscoverXamlProperties();
            this.ScaleAttentionStripeForDpi();
        }

        private static void OnStatusMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not StatusPanel panel)
            {
                return;
            }

            var oldMessage = e.OldValue as IStatusMessage;
            var newMessage = e.NewValue as IStatusMessage;

            if (newMessage is not null)
            {
                panel.DisplayStatusMessage(newMessage);
            }
            else
            {
                panel.SetVisualProperties(null);
            }

            bool shouldFadeOut = oldMessage?.FadeMessageOut == true || (e.OldValue != null && e.NewValue == null);
            bool shouldFadeIn = e.OldValue == null && e.NewValue != null;

            if (shouldFadeOut) panel.FadeOut();
            if (shouldFadeIn) panel.FadeIn();
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
}
