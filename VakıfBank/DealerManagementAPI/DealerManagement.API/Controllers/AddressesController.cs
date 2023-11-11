using System.Security.Claims;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DealerManagement.Operation.Cqrs.AddressCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressesController : ControllerBase
    {
        private IMediator mediator;

        public AddressesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet] 
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<AddressResponse>> GetAddress() { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new GetAddressQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("/api/companies/{company_id}/address")] 
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<AddressResponse>> GetAddressByCompanyId([FromRoute] int company_id) { 
            var operation = new GetAddressQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<AddressResponse>> CreateAddress([FromBody] AddressRequest model) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new CreateAddressCommand(model, company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<AddressResponse>> UpdateAddress([FromBody] AddressRequest model) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new UpdateAddressCommand(model, company_id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}