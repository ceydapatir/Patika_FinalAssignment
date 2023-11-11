namespace DealerManagement.Schema.Requests
{
    public class ProductRequest
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public int Stock { get; set; }
    }
    
    public class ProductUpdateRequest
    {
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
    }
}