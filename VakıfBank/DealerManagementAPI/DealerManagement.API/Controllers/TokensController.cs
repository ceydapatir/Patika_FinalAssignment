using DealerManagement.Base.Middleware;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static DealerManagement.Operation.Cqrs.TokenCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/tokens")]
    public class TokensController : ControllerBase
    {
        private IMediator mediator;

        public TokensController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpPost]
        public async Task<ApiResponse<TokenResponse>> Post([FromBody] TokenRequest request)
        {
            var operation = new CreateTokenCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }
    
        [TypeFilter(typeof(LogResourceFilter))]
        [TypeFilter(typeof(LogActionFilter))]
        [TypeFilter(typeof(LogAuthorizationFilter))]
        [TypeFilter(typeof(LogResourceFilter))]
        [TypeFilter(typeof(LogExceptionFilter))]
        [HttpGet("Test")]
        public ApiResponse Get()
        {
            return new ApiResponse();
        }
            
    }
}