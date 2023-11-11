using System.Security.Claims;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DealerManagement.Operation.Cqrs.MessageCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private IMediator mediator;

        public MessagesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<List<MessageResponse>>> GetMessageBox() { 
            var employee_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var operation = new GetAllMessagesQuery(employee_id);
            var result = await mediator.Send(operation);
            return result;
        }
        
        [HttpPost]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<MessageResponse>> CreateMessage([FromBody] MessageRequest model) { 
            var employee_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var operation = new CreateMessageCommand(model, employee_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{message_id}")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse> CreateMessage([FromRoute] int message_id) { 
            var operation = new DeleteMessageCommand(message_id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}