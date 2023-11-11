using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.OrderItemCqrs;

namespace DealerManagement.Operation.Query
{
    public class OrderItemQueryHandler :
        IRequestHandler<GetCartQuery, ApiResponse<List<OrderItemResponse>>>,
        IRequestHandler<GetOrderItemsByOrderIdQuery, ApiResponse<List<OrderItemResponse>>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public OrderItemQueryHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        
        public async Task<ApiResponse<List<OrderItemResponse>>> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            // Check
            Order order = await dbContext.Set<Order>().FirstOrDefaultAsync(x => x.Status.Equals("Cart") && x.CompanyId == request.CompanyId);
            if(order == null)
                return new ApiResponse<List<OrderItemResponse>>("Cart not found.");
            
            List<OrderItem> orderItems = await dbContext.Set<OrderItem>().Include(x => x.Product).Include(x => x.Order).Where(x => x.OrderId == order.Id).ToListAsync(cancellationToken);
            if(orderItems.Count() == 0)
                return new ApiResponse<List<OrderItemResponse>>("Cart is empty.");
            
            // Response
            List<OrderItemResponse> mapped = mapper.Map<List<OrderItemResponse>>(orderItems);
            return new ApiResponse<List<OrderItemResponse>>(mapped);
        }
        
        public async Task<ApiResponse<List<OrderItemResponse>>> Handle(GetOrderItemsByOrderIdQuery request, CancellationToken cancellationToken)
        {
            // Check
            Order order = await dbContext.Set<Order>().FirstOrDefaultAsync(x => !x.Status.Equals("Cart") && x.Id == request.OrderId);
            if(order == null)
                return new ApiResponse<List<OrderItemResponse>>("Order not found.");
            
            List<OrderItem> orderItems = await dbContext.Set<OrderItem>().Include(x => x.Product).Include(x => x.Order).Where(x => x.OrderId == request.OrderId).ToListAsync(cancellationToken);
            if(orderItems.Count() == 0)
                return new ApiResponse<List<OrderItemResponse>>("Cart is empty.");
            
            // Response
            List<OrderItemResponse> mapped = mapper.Map<List<OrderItemResponse>>(orderItems);
            return new ApiResponse<List<OrderItemResponse>>(mapped);
        }
    }
}