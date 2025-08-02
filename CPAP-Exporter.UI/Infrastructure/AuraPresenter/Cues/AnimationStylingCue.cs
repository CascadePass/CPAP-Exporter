namespace CascadePass.CPAPExporter
{
    public abstract class AnimationStylingCue : StylingCue
    {
        public TimeSpan? Duration { get; set; }

        public override bool IsEmpty => Duration == default;
    }
}
