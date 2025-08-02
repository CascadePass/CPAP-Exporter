namespace CascadePass.CPAPExporter
{
    public class ContentTransienceCue : StylingCue
    {
        public TimeSpan? DisplayDuration { get; set; }
        public bool? IsHoverPauseEnabled { get; set; }

        public override bool IsEmpty => this.DisplayDuration == null && this.IsHoverPauseEnabled == null;
    }
}
