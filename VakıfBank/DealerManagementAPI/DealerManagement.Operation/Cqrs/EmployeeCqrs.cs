
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class EmployeeCqrs
    {
        //Commands
        public record CreateEmployeeCommand(EmployeeRequest Model) : IRequest<ApiResponse<EmployeeResponse>>;
        public record UpdateEmployeeCommand(EmployeeUpdateRequest Model, int EmployeeId) : IRequest<ApiResponse<EmployeeResponse>>;
        public record DeleteEmployeeCommand(int EmployeeId) : IRequest<ApiResponse>;

        //Queries 
        public record GetEmployeesByCompanyIdQuery(int CompanyId) : IRequest<ApiResponse<List<EmployeeResponse>>>;
        public record GetEmployeeByIdQuery(int EmployeeId) : IRequest<ApiResponse<EmployeeResponse>>;
    }
}