using System.Security.Claims;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DealerManagement.Operation.Cqrs.SupplierCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/supplier")]
    public class SuppliersController : ControllerBase
    {
        private IMediator mediator;

        public SuppliersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpGet]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<SupplierResponse>> GetSupplier() { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var employee_role = (User.Identity as ClaimsIdentity).FindFirst("Role").Value;
            var operation = new GetSupplierQuery(company_id, employee_role);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<SupplierResponse>> UpdateOwnCompany([FromBody] SupplierUpdateRequest model) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new UpdateSupplierCommand(model, company_id);
            var result = await mediator.Send(operation);
            return result;
        }

    }
}