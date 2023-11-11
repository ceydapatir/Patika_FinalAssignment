using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class OrderCqrs
    {
        //Commands
        public record UpdateOrderCommand(int OrderId) : IRequest<ApiResponse<OrderResponse>>;
        public record DeleteOrderCommand(int OrderId, int CompanyId) : IRequest<ApiResponse<OrderResponse>>;

        //Queries
        public record GetCartQuery(int CompanyId) : IRequest<ApiResponse<OrderResponse>>;
        public record GetDealerOrdersQuery(int CompanyId) : IRequest<ApiResponse<List<OrderResponse>>>;
        public record GetSupplierOrdersQuery(int CompanyId) : IRequest<ApiResponse<List<OrderResponse>>>;
        public record GetOrderByIdQuery(int OrderId) : IRequest<ApiResponse<OrderResponse>>;
    }
}