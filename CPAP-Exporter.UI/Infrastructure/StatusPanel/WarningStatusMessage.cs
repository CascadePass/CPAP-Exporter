namespace CascadePass.CPAPExporter
{
    public class WarningStatusMessage : StatusPanelMessage
    {
        public WarningStatusMessage() : base(null, StatusMessageType.Warning) { }

        public WarningStatusMessage(object messageContent) : base(messageContent, StatusMessageType.Warning) { }
    }
}
