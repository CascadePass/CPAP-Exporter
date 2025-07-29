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
    /// A flexible content control that can display user messages.
    /// </summary>
    public partial class MessagePresenter : UserControl
    {
        #region Private Fields

        /// <summary>
        /// Represents a collection of <see cref="DependencyProperty"/> objects for properties set in XAML.
        /// </summary>
        /// <remarks>This field is intended to store a set of dependency properties that have been
        /// explicitly  set through XAML. It is used internally to manage and optimize property handling.</remarks>
        private readonly HashSet<DependencyProperty> xamlSetProperties;

        #endregion

        #region Dependency Properties

        public static new readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(MessagePresenter),
                new PropertyMetadata(null, OnContentChanged));

        public static readonly DependencyProperty MessageBorderThicknessProperty =
            DependencyProperty.Register(nameof(MessageBorderThickness), typeof(Thickness), typeof(MessagePresenter),
                new PropertyMetadata(new Thickness(1.5)));

        public static readonly DependencyProperty MessageBorderBrushProperty =
            DependencyProperty.Register(nameof(MessageBorderBrush), typeof(Brush), typeof(MessagePresenter),
                new PropertyMetadata(Brushes.Gold));

        public static readonly DependencyProperty ShowDropShadowProperty =
            DependencyProperty.Register(
                nameof(ShowDropShadow),
                typeof(bool),
                typeof(MessagePresenter),
                new PropertyMetadata(true));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(MessagePresenter),
                new PropertyMetadata(4.0));

        public static readonly DependencyProperty AttentionStripeWidthProperty =
            DependencyProperty.Register(nameof(AttentionStripeWidth), typeof(double), typeof(MessagePresenter),
                new FrameworkPropertyMetadata(4.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty AttentionStripeBrushProperty =
            DependencyProperty.Register(nameof(AttentionStripeBrush), typeof(Brush), typeof(MessagePresenter),
                new PropertyMetadata(new SolidColorBrush(Colors.Goldenrod), OnAttentionStripeBrushChanged));

        public static readonly DependencyProperty BackgroundBrushProperty =
            DependencyProperty.Register(nameof(BackgroundBrush), typeof(Brush), typeof(MessagePresenter),
                new PropertyMetadata(MessagePresenter.DefaultStatusBackgroundBrush));

        public static readonly DependencyProperty PulseStartColorProperty =
            DependencyProperty.Register(nameof(PulseStartColor), typeof(Color), typeof(MessagePresenter),
                new PropertyMetadata((Color)ColorConverter.ConvertFromString("#FFC891")));

        public static readonly DependencyProperty PulseEndColorProperty =
            DependencyProperty.Register(nameof(PulseEndColor), typeof(Color), typeof(MessagePresenter),
                new PropertyMetadata((Color)ColorConverter.ConvertFromString("#FFD971")));

        public static readonly DependencyProperty StatusMessageStyleProviderProperty =
            DependencyProperty.Register(nameof(StatusMessageStyleProvider), typeof(IStatusMessageStyleProvider), typeof(MessagePresenter),
                new PropertyMetadata(new DefaultStatusMessageStyleProvider()));

        public static readonly DependencyProperty OverrideBehaviorProperty =
            DependencyProperty.Register(
                nameof(OverrideBehavior),
                typeof(MessageOverrideBehavior),
                typeof(MessagePresenter),
                new PropertyMetadata(MessageOverrideBehavior.PreferLocalValues)
            );

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePresenter"/> class.
        /// </summary>
        /// <remarks>This constructor sets up the initial state of the <see cref="MessagePresenter"/>
        /// instance. It initializes the internal properties and prepares the component for use.</remarks>
        public MessagePresenter()
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

        #region Dependency Properties

        /// <summary>
        /// Gets or sets the status message displayed by the control.
        /// </summary>
        public new object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush used to render the border of a message.
        /// </summary>
        public Brush MessageBorderBrush
        {
            get => (Brush)GetValue(MessageBorderBrushProperty);
            set => SetValue(MessageBorderBrushProperty, value);
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

        /// <summary>
        /// Gets or sets a value indicating whether a drop shadow is displayed.
        /// </summary>
        public bool ShowDropShadow
        {
            get => (bool)GetValue(ShowDropShadowProperty);
            set => SetValue(ShowDropShadowProperty, value);
        }

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
        public Thickness MessageBorderThickness
        {
            get => (Thickness)GetValue(MessageBorderThicknessProperty);
            set => SetValue(MessageBorderThicknessProperty, value);
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
        public IStatusMessageStyleProvider StatusMessageStyleProvider
        {
            get => (IStatusMessageStyleProvider)GetValue(StatusMessageStyleProviderProperty);
            set => SetValue(StatusMessageStyleProviderProperty, value);
        }

        /// <summary>
        /// Gets or sets the behavior that determines how message properties are applied.
        /// </summary>
        public MessageOverrideBehavior OverrideBehavior
        {
            get => (MessageOverrideBehavior)GetValue(OverrideBehaviorProperty);
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

        public void AnimateAttentionStripeColor(MessagePresenter panel, SolidColorBrush oldBrush, SolidColorBrush newBrush)
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

        private void ForceShowMessage()
        {
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(0));
            this.BeginAnimation(OpacityProperty, fadeIn);
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
                MessagePresenter.ForegroundProperty,
                message?.ForegroundBrush,
                () => this.StatusMessageStyleProvider.GetForegroundBrush(message)
            );

            this.BackgroundBrush = this.ResolveValue(
                MessagePresenter.BackgroundBrushProperty,
                message?.BackgroundBrush,
                () => this.StatusMessageStyleProvider.GetBackgroundBrush(message)
            );

            this.MessageBorderBrush = this.ResolveValue(
                MessagePresenter.MessageBorderBrushProperty,
                message?.BorderBrush,
                () => this.StatusMessageStyleProvider.GetStatusPanelBorderBrush(message)
            );

            this.AttentionStripeBrush = this.ResolveValue(
                MessagePresenter.AttentionStripeBrushProperty,
                message?.AttentionStripeBrush,
                () => this.StatusMessageStyleProvider.GetAttentionStripeBrush(message)
            );

            this.AttentionStripeWidth = (double)this.ResolveValue(
                MessagePresenter.AttentionStripeWidthProperty,
                message?.AttentionStripeWidth,
                () => this.StatusMessageStyleProvider.GetAttentionStripeWidth(message)
            );

            this.ShowDropShadow = (bool)this.ResolveValue(
                MessagePresenter.ShowDropShadowProperty,
                message?.ShowDropShadow,
                () => this.StatusMessageStyleProvider.GetShowDropShadow(message)
            );

            this.CornerRadius = (double)this.ResolveValue(
                MessagePresenter.CornerRadiusProperty,
                message?.CornerRadius,
                () => this.StatusMessageStyleProvider.GetCornerRadius(message)
            );

            this.BorderThickness = (Thickness)this.ResolveValue(
                MessagePresenter.BorderThicknessProperty,
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
                MessagePresenter.MessageBorderBrushProperty,
                MessagePresenter.PulseStartColorProperty,
                MessagePresenter.PulseEndColorProperty,
                MessagePresenter.BackgroundBrushProperty,
                MessagePresenter.AttentionStripeBrushProperty,
                MessagePresenter.AttentionStripeWidthProperty,
                MessagePresenter.ShowDropShadowProperty,
                MessagePresenter.CornerRadiusProperty,
                MessagePresenter.MessageBorderThicknessProperty
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
            if (d is not MessagePresenter panel)
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
            if (d is not MessagePresenter panel)
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
