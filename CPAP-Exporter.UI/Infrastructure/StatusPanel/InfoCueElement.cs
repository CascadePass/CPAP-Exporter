namespace CascadePass.CPAPExporter
{
    public class InfoCueElement : ContentStylingCue
    {
        public InfoCueElement() : base(null, StatusMessageType.Info) { }

        public InfoCueElement(object messageContent) : base(messageContent, StatusMessageType.Info) { }
    }
}
