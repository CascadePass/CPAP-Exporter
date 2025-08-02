namespace CascadePass.CPAPExporter
{
    public interface IAuraContent
    {
        object Content { get; set; }

        ContentTransienceCue Transience { get; set; }

        IEnumerable<AnimationStylingCue> Animations { get; set; }

        BorderStylingCue Border { get; set; }
        AttentionStripeCue AttentionStripe { get; set; }
        ShadowStylingCue Shadow { get; set; }
        TextStylingCue BodyText { get; set; }
    }
}
