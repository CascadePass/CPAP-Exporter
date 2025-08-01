using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// A flexible content control that can display user messages.
    /// </summary>
    public partial class AuraPresenter : UserControl
    {
        #region Private Fields

        /// <summary>
        /// Represents a collection of <see cref="DependencyProperty"/> objects for properties set in XAML.
        /// </summary>
        /// <remarks>This field is intended to store a set of dependency properties that have been
        /// explicitly  set through XAML. It is used internally to manage and optimize property handling.</remarks>
        private readonly HashSet<DependencyProperty> xamlSetProperties;

        private DispatcherTimer autoDismissMessageTimer;

        #endregion

        #region Dependency Properties

        public static new readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(AuraPresenter),
                new PropertyMetadata(null, OnContentChanged));

        public static readonly DependencyProperty DisplayDurationProperty =
            DependencyProperty.Register(nameof(DisplayDuration), typeof(TimeSpan), typeof(AuraPresenter),
                new PropertyMetadata(TimeSpan.MaxValue));

        #region Border

        public static readonly DependencyProperty ContentBorderThicknessProperty =
            DependencyProperty.Register(nameof(ContentBorderThickness), typeof(Thickness), typeof(AuraPresenter),
                new PropertyMetadata(new Thickness(1.5)));

        public static readonly DependencyProperty ContentBorderBrushProperty =
            DependencyProperty.Register(nameof(ContentBorderBrush), typeof(Brush), typeof(AuraPresenter),
                new PropertyMetadata(Brushes.Gold));

        #endregion

        #region Shadow

        public static readonly DependencyProperty ShowDropShadowProperty =
            DependencyProperty.Register(
                nameof(ShowDropShadow),
                typeof(bool),
                typeof(AuraPresenter),
                new PropertyMetadata(true));

        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register(nameof(ShadowColor), typeof(Color), typeof(AuraPresenter),
                new PropertyMetadata(Colors.Black));

        public static readonly DependencyProperty ShadowBlurRadiusProperty =
            DependencyProperty.Register(nameof(ShadowBlurRadius), typeof(double), typeof(AuraPresenter),
                new PropertyMetadata(4.0));

        public static readonly DependencyProperty ShadowDepthProperty =
            DependencyProperty.Register(nameof(ShadowDepth), typeof(double), typeof(AuraPresenter),
                new PropertyMetadata(8.0));

        public static readonly DependencyProperty ShadowOpacityProperty =
            DependencyProperty.Register(nameof(ShadowOpacity), typeof(double), typeof(AuraPresenter),
                new PropertyMetadata(0.5));

        #endregion

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(AuraPresenter),
                new PropertyMetadata(4.0));

        #region Attention Stripe

        public static readonly DependencyProperty AttentionStripeWidthProperty =
            DependencyProperty.Register(nameof(AttentionStripeWidth), typeof(double), typeof(AuraPresenter),
                new FrameworkPropertyMetadata(4.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty AttentionStripeBrushProperty =
            DependencyProperty.Register(nameof(AttentionStripeBrush), typeof(Brush), typeof(AuraPresenter),
                new PropertyMetadata(new SolidColorBrush(Colors.Goldenrod), OnAttentionStripeBrushChanged));

        #endregion

        public static readonly DependencyProperty BackgroundBrushProperty =
            DependencyProperty.Register(nameof(BackgroundBrush), typeof(Brush), typeof(AuraPresenter),
                new PropertyMetadata(AuraPresenter.DefaultStatusBackgroundBrush));

        public static readonly DependencyProperty PulseStartColorProperty =
            DependencyProperty.Register(nameof(PulseStartColor), typeof(Color), typeof(AuraPresenter),
                new PropertyMetadata((Color)ColorConverter.ConvertFromString("#FFC891")));

        public static readonly DependencyProperty PulseEndColorProperty =
            DependencyProperty.Register(nameof(PulseEndColor), typeof(Color), typeof(AuraPresenter),
                new PropertyMetadata((Color)ColorConverter.ConvertFromString("#FFD971")));

        public static readonly DependencyProperty StylingCueProviderProperty =
            DependencyProperty.Register(nameof(StylingCueProvider), typeof(IStylingCueProvider), typeof(AuraPresenter),
                new PropertyMetadata(new DefaultStylingCueProvider()));

        public static readonly DependencyProperty OverrideBehaviorProperty =
            DependencyProperty.Register(
                nameof(OverrideBehavior),
                typeof(StylingCuePrecedence),
                typeof(AuraPresenter),
                new PropertyMetadata(StylingCuePrecedence.PreferLocalValues)
            );

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AuraPresenter"/> class.
        /// </summary>
        /// <remarks>This constructor sets up the initial state of the <see cref="AuraPresenter"/>
        /// instance. It initializes the internal properties and prepares the component for use.</remarks>
        public AuraPresenter()
        {
            this.xamlSetProperties = [];
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Represents the default background brush used for status indicators.
        /// </summary>
        /// <remarks>This brush is a linear gradient that transitions vertically from a light yellow color
        /// to a slightly darker shade. It is intended to provide a visually appealing background for status-related UI
        /// elements.</remarks>
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

        public DateTime? MessageBecameVisible { get; private set; }

        public TimeSpan TimeVisible => this.MessageBecameVisible.HasValue
            ? DateTime.Now - this.MessageBecameVisible.Value
            : TimeSpan.Zero;

        #region Dependency Properties

        /// <summary>
        /// Gets or sets the status message displayed by the control.
        /// </summary>
        public new object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public TimeSpan DisplayDuration
        {
            get => (TimeSpan)GetValue(DisplayDurationProperty);
            set => SetValue(DisplayDurationProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush used to render the border of a message.
        /// </summary>
        public Brush ContentBorderBrush
        {
            get => (Brush)GetValue(ContentBorderBrushProperty);
            set => SetValue(ContentBorderBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush used to paint the background of the control.
        /// </summary>
        public Brush BackgroundBrush
        {
            get => (Brush)GetValue(BackgroundBrushProperty);
            set => SetValue(BackgroundBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush used to render the attention stripe.
        /// </summary>
        public Brush AttentionStripeBrush
        {
            get => (Brush)GetValue(AttentionStripeBrushProperty);
            set => SetValue(AttentionStripeBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the width of the attention stripe.
        /// </summary>
        public double AttentionStripeWidth
        {
            get => (double)GetValue(AttentionStripeWidthProperty);
            set => SetValue(AttentionStripeWidthProperty, value);
        }

        #region Shadow Properties

        /// <summary>
        /// Gets or sets a value indicating whether a drop shadow is displayed.
        /// </summary>
        public bool ShowDropShadow
        {
            get => (bool)GetValue(ShowDropShadowProperty);
            set => SetValue(ShowDropShadowProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating the color of the shadow.
        /// </summary>
        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating the blur radius of the shadow.
        /// </summary>
        public double ShadowBlurRadius
        {
            get => (double)GetValue(ShadowBlurRadiusProperty);
            set => SetValue(ShadowBlurRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating the depth of the shadow.
        /// </summary>
        public double ShadowDepth
        {
            get => (double)GetValue(ShadowDepthProperty);
            set => SetValue(ShadowDepthProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating the depth of the shadow.
        /// </summary>
        public double ShadowOpacity
        {
            get => (double)GetValue(ShadowOpacityProperty);
            set => SetValue(ShadowOpacityProperty, value);
        }

        #endregion

        /// <summary>
        /// Gets or sets the radius of the corners for the element.
        /// </summary>
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets the thickness of the border surrounding the message.
        /// </summary>
        public Thickness ContentBorderThickness
        {
            get => (Thickness)GetValue(ContentBorderThicknessProperty);
            set => SetValue(ContentBorderThicknessProperty, value);
        }

        /// <summary>
        /// Gets or sets the starting color of the pulse animation.
        /// </summary>
        public Color PulseStartColor
        {
            get => (Color)GetValue(PulseStartColorProperty);
            set => SetValue(PulseStartColorProperty, value);
        }

        /// <summary>
        /// Gets or sets the ending color of the pulse animation.
        /// </summary>
        public Color PulseEndColor
        {
            get => (Color)GetValue(PulseEndColorProperty);
            set => SetValue(PulseEndColorProperty, value);
        }

        /// <summary>
        /// Gets or sets the provider responsible for determining the appearance of status messages.
        /// </summary>
        public IStylingCueProvider StylingCueProvider
        {
            get => (IStylingCueProvider)GetValue(StylingCueProviderProperty);
            set => SetValue(StylingCueProviderProperty, value);
        }

        /// <summary>
        /// Gets or sets the behavior that determines how message properties are applied.
        /// </summary>
        public StylingCuePrecedence OverrideBehavior
        {
            get => (StylingCuePrecedence)GetValue(OverrideBehaviorProperty);
            set => SetValue(OverrideBehaviorProperty, value);
        }

        #endregion

        #endregion

        #region Methods

        #region Animation

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

        public void PulseBorderColor(Color fromColor, Color toColor, TimeSpan duration)
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

        public void AnimateAttentionStripeColor(AuraPresenter panel, SolidColorBrush oldBrush, SolidColorBrush newBrush)
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

        #endregion

        public void Close()
        {
            if (this.Content is not IStylingCue auraContent)
            {
                return;
            }

            if (auraContent.AnimationCues?.Any(cue => cue is FadeOutCue) == true || this.StylingCueProvider?.GetFadeOut(auraContent) == true)
            {
                this.FadeOut();
            }

            this.Visibility = Visibility.Collapsed;
        }

        private void ForceShowMessage()
        {
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(0));
            this.BeginAnimation(OpacityProperty, fadeIn);
        }

        private void StartAutoCloseTimer()
        {
            this.autoDismissMessageTimer?.Stop();

            if (this.DisplayDuration < TimeSpan.MaxValue)
            {
                this.autoDismissMessageTimer = new DispatcherTimer
                {
                    Interval = this.DisplayDuration
                };

                this.autoDismissMessageTimer.Tick += (s, e) =>
                {
                    this.autoDismissMessageTimer.Stop();
                    this.autoDismissMessageTimer = null;

                    this.Close();
                };

                this.autoDismissMessageTimer.Start();
            }
        }

        private void DisplayStatusMessage(IStylingCue auraContent)
        {
            if (auraContent == null)
            {
                return;
            }

            // Set properties from the message
            this.SetVisualProperties(auraContent);
            this.SetTemporalProperties(auraContent);

            // Handle fade in/out and pulse border
            if (!auraContent.AnimationCues?.Any(cue => cue is FadeInCue) ?? true)
            {
                this.ForceShowMessage();
            }

            var pulseCue = (PulseBorderCue)auraContent.AnimationCues?.FirstOrDefault(cue => cue is PulseBorderCue);
            if (pulseCue is not null)
            {
                this.PulseBorderColor(this.PulseStartColor, this.PulseEndColor, pulseCue.Duration ?? TimeSpan.FromMilliseconds(400));
            }
        }

        private void SetVisualProperties(IStylingCue auraContent)
        {
            this.Foreground = this.ResolveValue(
                AuraPresenter.ForegroundProperty,
                auraContent?.TextCue?.ForegroundBrush,
                () => this.StylingCueProvider?.GetForegroundBrush(auraContent)
            );

            this.BackgroundBrush = this.ResolveValue(
                AuraPresenter.BackgroundBrushProperty,
                auraContent?.BorderCue?.BackgroundBrush,
                () => this.StylingCueProvider?.GetBackgroundBrush(auraContent)
            );

            this.ContentBorderBrush = this.ResolveValue(
                AuraPresenter.ContentBorderBrushProperty,
                auraContent?.BorderCue?.BorderBrush,
                () => this.StylingCueProvider?.GetStatusPanelBorderBrush(auraContent)
            );

            this.AttentionStripeBrush = this.ResolveValue(
                AuraPresenter.AttentionStripeBrushProperty,
                auraContent?.AttentionStripeCue?.Brush,
                () => this.StylingCueProvider?.GetAttentionStripeBrush(auraContent)
            );


            this.AttentionStripeWidth = (double)this.ResolveValue(
                AuraPresenter.AttentionStripeWidthProperty,
                auraContent?.AttentionStripeCue?.Width,
                () => this.StylingCueProvider?.GetAttentionStripeWidth(auraContent)
            );

            this.ShowDropShadow = (bool)this.ResolveValue(
                AuraPresenter.ShowDropShadowProperty,
                !auraContent?.ShadowCue?.IsEmpty,
                () => this.StylingCueProvider?.GetShowDropShadow(auraContent)
            );

            this.CornerRadius = (double)this.ResolveValue(
                AuraPresenter.CornerRadiusProperty,
                auraContent?.BorderCue?.CornerRadius,
                () => this.StylingCueProvider?.GetCornerRadius(auraContent)
            );

            this.ContentBorderThickness = (Thickness)this.ResolveValue(
                AuraPresenter.BorderThicknessProperty,
                auraContent?.BorderCue?.BorderThickness,
                () => this.StylingCueProvider?.GetBorderThickness(auraContent)
            );

            this.ShadowColor = (Color)this.ResolveValue(
                AuraPresenter.ShadowColorProperty,
                auraContent?.ShadowCue?.ShadowColor,
                () => this.StylingCueProvider?.GetShadowColor(auraContent)
            );

            // Set the inside corner radius to zero for the attention stripe
            this.AttentionStripe.CornerRadius = new CornerRadius(this.CornerRadius, 0, 0, this.CornerRadius);
            this.AttentionStripe.Margin = new Thickness(-this.BorderThickness.Left, 0, 0, 0);
        }

        private void SetTemporalProperties(IStylingCue auraContent)
        {
            var displayDurationValue = (TimeSpan?)this.ResolveValue(
                AuraPresenter.DisplayDurationProperty,
                auraContent?.DisplayDuration,
                () => this.StylingCueProvider?.GetDisplayDuration(auraContent)
            );

            if (displayDurationValue.HasValue)
            {
                this.DisplayDuration = displayDurationValue.Value;
            }
            else
            {
                this.DisplayDuration = TimeSpan.MaxValue;
            }
        }

        private T ResolveValue<T>(DependencyProperty property, T auraContentValue, Func<T> styleProviderValue)
        {
            bool hasLocalValue = this.xamlSetProperties.Contains(property);

            return this.OverrideBehavior switch
            {
                StylingCuePrecedence.PreferLocalValues => hasLocalValue
                                        ? (T)GetValue(property)
                                        : auraContentValue ?? styleProviderValue(),

                StylingCuePrecedence.PreferMessageValues => auraContentValue ?? (hasLocalValue ? (T)GetValue(property) : styleProviderValue()),

                _ => hasLocalValue
                                        ? (T)GetValue(property)
                                        : auraContentValue ?? styleProviderValue(),
            };
        }

        private void DiscoverXamlProperties()
        {
            var properties = new[]
            {
                AuraPresenter.ContentBorderBrushProperty,
                AuraPresenter.PulseStartColorProperty,
                AuraPresenter.PulseEndColorProperty,
                AuraPresenter.BackgroundBrushProperty,
                AuraPresenter.AttentionStripeBrushProperty,
                AuraPresenter.AttentionStripeWidthProperty,
                AuraPresenter.ShowDropShadowProperty,
                AuraPresenter.CornerRadiusProperty,
                AuraPresenter.ContentBorderThicknessProperty
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

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            #region Sanity checks

            if (d is not AuraPresenter panel)
            {
                return;
            }

            if (AuraPresenter.Equals(e.OldValue, e.NewValue))
            {
                return;
            }

            #endregion

            if (e.NewValue is null)
            {
                panel.MessageBecameVisible = null;
                return;
            }

            var oldMessage = e.OldValue as IStylingCue;
            var newMessage = e.NewValue as IStylingCue;

            //bool shouldFadeOut = oldMessage?.FadeMessageOut == true || (e.OldValue != null && e.NewValue == null);
            bool shouldFadeIn = e.OldValue == null && e.NewValue != null;

            if (shouldFadeIn) panel.FadeIn();

            if (newMessage is not null)
            {
                panel.DisplayStatusMessage(newMessage);
            }
            else
            {
                panel.SetTemporalProperties(null);
                panel.SetVisualProperties(null);
            }

            panel.Visibility = Visibility.Visible;
            panel.MessageBecameVisible = DateTime.Now;
            panel.StartAutoCloseTimer();

            //if (shouldFadeOut) panel.FadeOut();
        }

        private static void OnAttentionStripeBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not AuraPresenter panel)
            {
                return;
            }

            var oldBrush = e.OldValue as SolidColorBrush;
            var newBrush = e.NewValue as SolidColorBrush;

            panel.AnimateAttentionStripeColor(panel, oldBrush, newBrush);
        }

        #endregion
    }
}
