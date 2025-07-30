namespace CascadePass.CPAPExporter
{
    public class WarningCueElement : ContentStylingCue
    {
        public WarningCueElement() : base(null, StatusMessageType.Warning) { }

        public WarningCueElement(object messageContent) : base(messageContent, StatusMessageType.Warning) { }
    }
}
