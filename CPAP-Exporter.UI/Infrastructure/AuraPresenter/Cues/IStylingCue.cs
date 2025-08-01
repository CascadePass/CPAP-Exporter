using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public interface IStylingCue
    {
        CuedContentType ContentType { get; }
        TimeSpan? DisplayDuration { get; set; }

        object Content { get; set; }


        IEnumerable<AnimationStylingCue> AnimationCues { get; set; }

        BorderStylingCue BorderCue { get; set; }
        AttentionStripeCue AttentionStripeCue { get; set; }
        ShadowStylingCue ShadowCue { get; set; }
        TextStylingCue TextCue { get; set; }
    }

    public abstract class StylingCue : Observable
    {
        public virtual bool IsEmpty => true;
    }

    public class ShadowStylingCue : StylingCue
    {
        public Color? ShadowColor { get; set; }
        public double? ShadowOpacity { get; set; }
        public double? ShadowBlurRadius { get; set; }
        public double? ShadowDepth { get; set; }

        public override bool IsEmpty =>
            ShadowColor != null ||
            ShadowOpacity != null ||
            ShadowBlurRadius != null ||
            ShadowDepth != null
        ;
    }

    public class AttentionStripeCue : StylingCue
    {
        public Brush Brush { get; set; }
        public double? Width { get; set; }

        public override bool IsEmpty =>
            Brush != null ||
            Width != null
        ;
    }

    public class BorderStylingCue : StylingCue
    {
        public Brush BorderBrush { get; set; }
        public Brush BackgroundBrush { get; set; }
        public double? CornerRadius { get; set; }
        public Thickness? BorderThickness { get; set; }

        public override bool IsEmpty =>
            BorderBrush != null ||
            BackgroundBrush != null ||
            CornerRadius != null ||
            BorderThickness != null
        ;
    }

    public class TextStylingCue : StylingCue
    {
        public Brush ForegroundBrush { get; set; }
        public FontFamily FontFamily { get; set; }
        public double? FontSize { get; set; }
        public FontWeight? FontWeight { get; set; }
        public FontStyle? FontStyle { get; set; }

        public override bool IsEmpty =>
            ForegroundBrush != null ||
            FontFamily != null ||
            FontSize != null ||
            FontWeight != null ||
            FontStyle != null
        ;
    }




    public abstract class AnimationStylingCue : StylingCue
    {
        public TimeSpan? Duration { get; set; }

        public override bool IsEmpty => Duration == default;
    }

    public class PulseBorderCue : AnimationStylingCue
    {
        public Color FromColor { get; set; }
        public Color ToColor { get; set; }

        public override bool IsEmpty =>
            Duration == default ||
            FromColor == ToColor
        ;
    }

    public class FadeInCue : AnimationStylingCue
    {
        public double FromOpacity { get; set; }
        public double ToOpacity { get; set; }

        public override bool IsEmpty =>
            Duration == default ||
            FromOpacity == ToOpacity
        ;
    }

    public class FadeOutCue : AnimationStylingCue
    {
        public double FromOpacity { get; set; }
        public double ToOpacity { get; set; }
        public override bool IsEmpty =>
            Duration == default ||
            FromOpacity == ToOpacity
        ;
    }
}
