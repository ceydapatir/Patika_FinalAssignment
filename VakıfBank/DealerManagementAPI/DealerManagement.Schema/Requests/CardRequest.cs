namespace DealerManagement.Schema.Requests
{
    public class CardRequest
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpireDate { get; set; }
    }
}