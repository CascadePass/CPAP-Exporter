namespace CascadePass.CPAPExporter
{
    public class FadeInCue : AnimationStylingCue
    {
        public double FromOpacity { get; set; }
        public double ToOpacity { get; set; }

        public override bool IsEmpty =>
            Duration == default &&
            FromOpacity == ToOpacity
        ;
    }
}
