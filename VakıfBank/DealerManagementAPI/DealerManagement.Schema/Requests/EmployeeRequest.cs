
namespace DealerManagement.Schema.Requests
{
    public class EmployeeRequest
    {
        public int CompanyId { get; set; }
        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }

    public class EmployeeUpdateRequest
    {
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}