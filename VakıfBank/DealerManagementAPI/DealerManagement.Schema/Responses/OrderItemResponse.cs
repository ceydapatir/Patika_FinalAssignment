
namespace DealerManagement.Schema.Responses
{
    public class OrderItemResponse
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
    }
}