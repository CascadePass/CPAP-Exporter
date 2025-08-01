using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    //TODO: Rename to AuraContent?

    public class ContentStylingCue : Observable, IStylingCue
    {
        private object content;

        public ContentStylingCue() { }

        public ContentStylingCue(object displayContent) {
            Content = displayContent;
        }

        public ContentStylingCue(object displayContent, CuedContentType messageType)
        {
            Content = displayContent;
            ContentType = messageType;
        }

        public CuedContentType ContentType { get; set; }

        public object Content {
            get => content;
            set => SetPropertyValue(ref content, value, nameof(content));
        }

        public TimeSpan? DisplayDuration { get; set; }
        public IEnumerable<AnimationStylingCue> AnimationCues { get; set; }
        public BorderStylingCue BorderCue { get; set; }
        public AttentionStripeCue AttentionStripeCue { get; set; }
        public ShadowStylingCue ShadowCue { get; set; }
        public TextStylingCue TextCue { get; set; }
    }
}
