using System.Security.Claims;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DealerManagement.Operation.Cqrs.PaymentCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentsController : ControllerBase
    {
        private IMediator mediator;

        public PaymentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("/api/orders/{order_id}/payment")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<PaymentResponse>> GetPaymentByOrderId([FromRoute] int order_id) { 
            var operation = new GetPaymentByOrderIdQuery(order_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse<PaymentResponse>> CreatePayment(PaymentRequest model) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new CreatePaymentCommand(model, company_id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}