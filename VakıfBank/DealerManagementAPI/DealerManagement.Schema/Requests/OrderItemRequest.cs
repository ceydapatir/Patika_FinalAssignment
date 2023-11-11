
namespace DealerManagement.Schema.Requests
{
    public class OrderItemRequest
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }

    public class OrderItemUpdateRequest
    {
        public int Amount { get; set; }
    }
}