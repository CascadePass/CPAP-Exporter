namespace CascadePass.CPAPExporter
{
    public class AuraContent : Observable, IAuraContent
    {
        private object content;

        public AuraContent() { }

        public AuraContent(object displayContent) {
            Content = displayContent;
        }

        public object Content {
            get => content;
            set => SetPropertyValue(ref content, value, nameof(content));
        }

        public ContentTransienceCue Transience { get; set; }

        public IEnumerable<AnimationStylingCue> Animations { get; set; }
        public BorderStylingCue Border { get; set; }
        public AttentionStripeCue AttentionStripe { get; set; }
        public ShadowStylingCue Shadow { get; set; }
        public TextStylingCue BodyText { get; set; }
    }
}
