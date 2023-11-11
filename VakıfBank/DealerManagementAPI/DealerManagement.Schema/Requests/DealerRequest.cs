namespace DealerManagement.Schema.Requests
{
    public class DealerRequest
    {   
        public string CompanyName { get; set; }
        public string Mail { get; set; }
        public double ProfitMargin { get; set; }
        public DateTime ContractDeadline { get; set; }
    }
    public class DealerUpdateRequest
    {   
        public string? Mail { get; set; }
    }
    public class UpdateProfitMarginRequest
    {   
        public double ProfitMargin { get; set; }
    }
}