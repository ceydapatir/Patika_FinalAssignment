using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class OrderItemCqrs
    {
        //Commands
        public record CreateOrderItemCommand(OrderItemRequest Model, int CompanyId) : IRequest<ApiResponse<OrderItemResponse>>;
        public record UpdateOrderItemCommand(OrderItemUpdateRequest Model, int OrderItemId, int CompanyId) : IRequest<ApiResponse<OrderItemResponse>>;
        public record DeleteAllOrderItemCommand(int CompanyId) : IRequest<ApiResponse>;
        public record DeleteOrderItemCommand(int OrderItemId, int CompanyId) : IRequest<ApiResponse>;

        //Queries
        public record GetCartQuery(int CompanyId) : IRequest<ApiResponse<List<OrderItemResponse>>>;
        public record GetOrderItemsByOrderIdQuery(int OrderId) : IRequest<ApiResponse<List<OrderItemResponse>>>;
    }
}