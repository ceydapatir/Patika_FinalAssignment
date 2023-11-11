using DealerManagement.Data.Domain;

namespace DealerManagement.Schema.Responses
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public double KDV { get; set; }
        public int Stock { get; set; }
    }
}