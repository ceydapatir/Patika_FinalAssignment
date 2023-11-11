namespace DealerManagement.Schema.Responses
{
    public class PaymentResponse
    {
        public string PaymentType { get; set; }
        public int? CardId { get; set; }
        public int CompanyId { get; set; }
        public int SupplierId { get; set; }
        public string Status { get; set; }
        public string? PaymentDate { get; set; } 
    }
}