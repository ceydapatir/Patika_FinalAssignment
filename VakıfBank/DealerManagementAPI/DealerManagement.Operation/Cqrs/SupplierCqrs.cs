
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class SupplierCqrs
    {
        //Commands
        public record UpdateSupplierCommand(SupplierUpdateRequest Model, int CompanyId) : IRequest<ApiResponse<SupplierResponse>>;

        //Queries   
        public record GetSupplierQuery(int CompanyId, string EmployeeRole) : IRequest<ApiResponse<SupplierResponse>>;         
    }
}