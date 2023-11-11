using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Responses;
using DealerManagement.Schema.Requests;
using System.Security.Claims;
using static DealerManagement.Operation.Cqrs.CardCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/cards")]
    public class CardsController : ControllerBase
    {
        private IMediator mediator;

        public CardsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet] 
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse<List<CardResponse>>> GetAllCards() { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new GetAllCardsQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("{card_id}")] 
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse<CardResponse>> GetCardById([FromRoute] int card_id) { 
            var operation = new GetCardByIdQuery(card_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse<CardResponse>> CreateCard([FromBody] CardRequest model) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new CreateCardCommand(model, company_id);
            var result = await mediator.Send(operation);
            return result;
        }
        
        [HttpDelete("{card_id}")]
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse> DeleteCard([FromRoute] int card_id) { 
            var operation = new DeleteCardCommand(card_id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}