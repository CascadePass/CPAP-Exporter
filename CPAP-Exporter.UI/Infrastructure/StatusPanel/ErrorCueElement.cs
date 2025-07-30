namespace CascadePass.CPAPExporter
{
    public class ErrorCueElement : ContentStylingCue
    {
        public ErrorCueElement() : base(null, StatusMessageType.Error) { }

        public ErrorCueElement(object messageContent) : base(messageContent, StatusMessageType.Error) { }
    }
}
