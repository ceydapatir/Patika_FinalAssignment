using System.Security.Claims;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DealerManagement.Operation.Cqrs.DealerCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/dealers")]
    public class DealersController : ControllerBase
    {
        private IMediator mediator;

        public DealersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpGet("/api/dealer")]
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse<DealerResponse>> GetOwnCompany() { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new GetCompanyQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<DealerResponse>>> GetAllDealers() { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new GetAllDealersQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("{company_id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<DealerResponse>> GetDealerById([FromRoute] int company_id) { 
            var operation = new GetDealerByIdQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<DealerResponse>> CreateDealer([FromBody] DealerRequest model) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new CreateDealerCommand(model, company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("/api/dealer")]
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse<DealerResponse>> UpdateDealer([FromBody] DealerUpdateRequest model) {
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new UpdateDealerCommand(model, company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("{company_id}")] 
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<DealerResponse>> UpdateDealerProfitMargin([FromRoute] int company_id, [FromBody] UpdateProfitMarginRequest model) {
            var operation = new UpdateProfitMarginCommand(model, company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{company_id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> DeleteDealer([FromRoute] int company_id) { 
            var operation = new DeleteDealerCommand(company_id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}