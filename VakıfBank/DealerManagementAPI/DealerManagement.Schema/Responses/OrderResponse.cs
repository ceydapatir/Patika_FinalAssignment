
namespace DealerManagement.Schema.Responses
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public int CompanyId { get; set; }
        public double Price { get; set; }
        public double KDVPrice { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }
    }
}