
namespace DealerManagement.Schema.Responses
{
    public class MessageResponse
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageContent {get; set;}
        public string MessageDate { get; set; }
    }
}