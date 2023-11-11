namespace DealerManagement.Schema.Responses
{
    public class AddressResponse
    {
        public int AddressId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string PostalCode { get; set; }
    }
}