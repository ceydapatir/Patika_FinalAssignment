using System.Security.Claims;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DealerManagement.Operation.Cqrs.OrderCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private IMediator mediator;

        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("/api/cart")]
        [Authorize(Roles = "dealer")] 
        public async Task<ApiResponse<OrderResponse>> GetCart() { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new GetCartQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet]
        [Authorize(Roles = "admin, dealer")] 
        public async Task<ApiResponse<List<OrderResponse>>> GetDealerOrders() { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var employee_role = (User.Identity as ClaimsIdentity).FindFirst("Role").Value;
            if(employee_role.Equals("admin")){
                var operation = new GetSupplierOrdersQuery(company_id);
                var result = await mediator.Send(operation);
                return result;
            }else{
                var operation = new GetDealerOrdersQuery(company_id);
                var result = await mediator.Send(operation);
                return result;
            }
        }

        [HttpGet("{order_id}")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<OrderResponse>> GetOrderById([FromRoute] int order_id) { 
            var operation = new GetOrderByIdQuery(order_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("{order_id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<OrderResponse>> ConfirmOrder([FromRoute] int order_id) { 
            var operation = new UpdateOrderCommand(order_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{order_id}")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<OrderResponse>> CancelOrder([FromRoute] int order_id) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new DeleteOrderCommand(order_id, company_id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}