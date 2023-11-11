using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.OrderCqrs;

namespace DealerManagement.Operation.Query
{
    public class OrderQueryHandler :
        IRequestHandler<GetDealerOrdersQuery, ApiResponse<List<OrderResponse>>>,
        IRequestHandler<GetCartQuery, ApiResponse<OrderResponse>>,
        IRequestHandler<GetSupplierOrdersQuery, ApiResponse<List<OrderResponse>>>,
        IRequestHandler<GetOrderByIdQuery, ApiResponse<OrderResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public OrderQueryHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<OrderResponse>>> Handle(GetDealerOrdersQuery request, CancellationToken cancellationToken)
        { 
            // Check
            List<Order> orders = await dbContext.Set<Order>().Where(x => (x.CompanyId == request.CompanyId) && !x.Status.Equals("Cart")).ToListAsync(cancellationToken);
            if(orders.Count() == 0)
                return new ApiResponse<List<OrderResponse>>("Order not found.");
            
            // Response
            List<OrderResponse> mapped = mapper.Map<List<OrderResponse>>(orders);
            return new ApiResponse<List<OrderResponse>>(mapped);
        }

        public async Task<ApiResponse<OrderResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
        { 
            // Check
            Order order = await dbContext.Set<Order>().FirstOrDefaultAsync(x => (x.CompanyId == request.CompanyId) && x.Status.Equals("Cart"), cancellationToken);
            if(order == null)
                return new ApiResponse<OrderResponse>("Cart not found.");
            
            // Response
            OrderResponse mapped = mapper.Map<OrderResponse>(order);
            return new ApiResponse<OrderResponse>(mapped);
        }

        public async Task<ApiResponse<List<OrderResponse>>> Handle(GetSupplierOrdersQuery request, CancellationToken cancellationToken)
        { 
            // Check
            List<Order> orders = new();
            List<Company> companies = await dbContext.Set<Company>().Where(x => x.Dealer.SupplierId == request.CompanyId).ToListAsync(cancellationToken);
            foreach (var company in companies)
            {
                List<Order> dealerorders = await dbContext.Set<Order>().Where(x => (x.CompanyId == company.Id) && !x.Status.Equals("Cart")).ToListAsync(cancellationToken);
                orders.AddRange(dealerorders);
            }
            if(orders.Count() == 0)
                return new ApiResponse<List<OrderResponse>>("Order not found.");
            
            // Response
            List<OrderResponse> mapped = mapper.Map<List<OrderResponse>>(orders);
            return new ApiResponse<List<OrderResponse>>(mapped);
        }

        public async Task<ApiResponse<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            // Check
            Order order = await unitOfWork.OrderRepository.GetById(cancellationToken, request.OrderId);
            if(order == null)
                return new ApiResponse<OrderResponse>("Order not found.");
            
            // Response
            OrderResponse mapped = mapper.Map<OrderResponse>(order);
            return new ApiResponse<OrderResponse>(mapped);
        }

    }
}