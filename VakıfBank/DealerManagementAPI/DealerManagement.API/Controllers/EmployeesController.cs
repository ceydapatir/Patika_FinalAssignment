using System.Security.Claims;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DealerManagement.Operation.Cqrs.EmployeeCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private IMediator mediator;

        public EmployeesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("/api/companies/{company_id}/employees")] 
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<List<EmployeeResponse>>> GetEmployeesByCompanyId([FromRoute] int company_id) { 
            var operation = new GetEmployeesByCompanyIdQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("{employee_id}")]
        [Authorize(Roles = "dealer, admin")]
        public async Task<ApiResponse<EmployeeResponse>> GetEmployeeById([FromRoute] int employee_id) { 
            var operation = new GetEmployeeByIdQuery(employee_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("/api/employee")]
        [Authorize(Roles = "dealer, admin")]
        public async Task<ApiResponse<EmployeeResponse>> GetProfile() { 
            var employee_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var operation = new GetEmployeeByIdQuery(employee_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<EmployeeResponse>> CreateAccount([FromBody] EmployeeRequest model) { 
            var operation = new CreateEmployeeCommand(model);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("/api/employee")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<EmployeeResponse>> UpdateProfile([FromBody] EmployeeUpdateRequest model) { 
            var employee_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var operation = new UpdateEmployeeCommand(model, employee_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{employee_id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse>  DeleteEmployee([FromRoute] int employee_id) { 
            var operation = new DeleteEmployeeCommand(employee_id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
