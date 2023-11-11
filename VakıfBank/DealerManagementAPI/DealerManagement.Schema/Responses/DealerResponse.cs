
namespace DealerManagement.Schema.Responses
{
    public class DealerResponse
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Mail { get; set; }
        public double ProfitMargin { get; set; }
        public string ContractDeadline { get; set; }
    }
}