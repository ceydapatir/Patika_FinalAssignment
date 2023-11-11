using System.Security.Claims;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DealerManagement.Operation.Cqrs.OrderItemCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/order-items")]
    public class OrderItemsController : ControllerBase
    {
        private IMediator mediator;

        public OrderItemsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet] 
        [Authorize(Roles = "dealer")] 
        public async Task<ApiResponse<List<OrderItemResponse>>> GetCartItems() { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new GetCartQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("/api/orders/{order_id}/order-items")] 
        [Authorize(Roles = "admin, dealer")] 
        public async Task<ApiResponse<List<OrderItemResponse>>> GetOrderItemsByOrderId([FromRoute] int order_id) { 
            var operation = new GetOrderItemsByOrderIdQuery(order_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse<OrderItemResponse>> CreateOrderItem([FromBody] OrderItemRequest model) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new CreateOrderItemCommand(model, company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("{orderitem_id}")]
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse<OrderItemResponse>> UpdateOrderItemAmount([FromRoute] int orderitem_id, [FromBody] OrderItemUpdateRequest model) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new UpdateOrderItemCommand(model, orderitem_id, company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete]
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse> DeleteAllProductsFromCart() { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new DeleteAllOrderItemCommand(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{orderitem_id}")]
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse> DeleteOrderItemFromCart([FromRoute] int orderitem_id) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new DeleteOrderItemCommand(orderitem_id, company_id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}