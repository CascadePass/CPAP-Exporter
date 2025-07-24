namespace CascadePass.CPAPExporter
{
    public class InfoStatusMessage : StatusPanelMessage
    {
        public InfoStatusMessage() : base(null, StatusMessageType.Info) { }

        public InfoStatusMessage(object messageContent) : base(messageContent, StatusMessageType.Info) { }
    }
}
