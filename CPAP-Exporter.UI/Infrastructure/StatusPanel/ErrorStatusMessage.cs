namespace CascadePass.CPAPExporter
{
    public class ErrorStatusMessage : StatusPanelMessage
    {
        public ErrorStatusMessage() : base(null, StatusMessageType.Error) { }

        public ErrorStatusMessage(object messageContent) : base(messageContent, StatusMessageType.Error) { }
    }
}
