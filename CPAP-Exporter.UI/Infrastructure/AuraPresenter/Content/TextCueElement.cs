namespace CascadePass.CPAPExporter
{
    public class TextCueElement : ContentStylingCue
    {
        public TextCueElement()
        {
        }

        public TextCueElement(object displayContent) : base(displayContent)
        {
        }

        public TextCueElement(object displayContent, CuedContentType messageType) : base(displayContent, messageType)
        {
        }
    }
}
