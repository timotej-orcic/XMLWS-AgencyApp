namespace XML_WS_AgencyApp.Models_DTO
{
    public class Message_DTO
    {
        public long SenderAgentMainServerId { get; set; }

        public long ReceiverUserMainServerId { get; set; }

        public string Content { get; set; }
    }
}