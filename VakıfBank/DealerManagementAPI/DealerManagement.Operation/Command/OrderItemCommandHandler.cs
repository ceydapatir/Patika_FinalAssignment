
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Operation.Validation;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.OrderItemCqrs;
namespace DealerManagement.Operation.Command
{
    public class OrderItemCommandHandler:
        IRequestHandler<CreateOrderItemCommand, ApiResponse<OrderItemResponse>>,
        IRequestHandler<UpdateOrderItemCommand, ApiResponse<OrderItemResponse>>,
        IRequestHandler<DeleteOrderItemCommand, ApiResponse>,
        IRequestHandler<DeleteAllOrderItemCommand, ApiResponse>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public OrderItemCommandHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<OrderItemResponse>> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            // Validation
            OrderItemValidator validator = new(dbContext);
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<OrderItemResponse>(result.Errors[0].ToString());

            // Check
            Order order = await dbContext.Set<Order>().AsQueryable().FirstOrDefaultAsync(x => x.CompanyId == request.CompanyId && x.Status.Equals("Cart"), cancellationToken);
            if(order == null)
                return new ApiResponse<OrderItemResponse>("Order not found.");
            
            OrderItem orderItem = await dbContext.Set<OrderItem>().FirstOrDefaultAsync(x => x.OrderId == order.Id && x.ProductId == request.Model.ProductId, cancellationToken);
            if(orderItem is not null)
                return new ApiResponse<OrderItemResponse>("OrderItem already exist.");
            
            var company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Dealer");
            CompanyStock companyStock = await dbContext.Set<CompanyStock>().FirstOrDefaultAsync(x => x.CompanyId == company.Dealer.SupplierId && x.ProductId == request.Model.ProductId, cancellationToken);
            if(companyStock.Stock < request.Model.Amount)
                return new ApiResponse<OrderItemResponse>("Supplier stock not enough.");

            // Create
            orderItem = new OrderItem(){ OrderId = order.Id, ProductId = request.Model.ProductId, Amount = request.Model.Amount};
            orderItem = await unitOfWork.OrderItemRepository.InsertAndGetEntity(cancellationToken, orderItem, "Product");

            // Update Supplier Stock and Cart
            decimal ProfitMargin = (company.Dealer.ProfitMargin + 100) / 100;
            companyStock.Stock -= orderItem.Amount;
            
            order.UnitPriceSum += orderItem.Product.UnitPrice * ProfitMargin * orderItem.Amount;
            order.KDVPriceSum += orderItem.Product.KDV / 100 * orderItem.Product.UnitPrice * ProfitMargin * orderItem.Amount;
            await dbContext.SaveChangesAsync(cancellationToken);
            order.TotalPrice = order.UnitPriceSum + order.KDVPriceSum;
            await dbContext.SaveChangesAsync(cancellationToken);
            
            // Response
            var response = mapper.Map<OrderItemResponse>(orderItem);
            return new ApiResponse<OrderItemResponse>(response);
        }

        public async Task<ApiResponse<OrderItemResponse>> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            // Validation
            OrderItemUpdateValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<OrderItemResponse>(result.Errors[0].ToString());

            // Check
            OrderItem orderItem = await unitOfWork.OrderItemRepository.GetById(cancellationToken, request.OrderItemId, "Product", "Order");
            var company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Dealer");
            CompanyStock companyStock = await dbContext.Set<CompanyStock>().FirstOrDefaultAsync(x => x.CompanyId == company.Dealer.SupplierId && x.ProductId == orderItem.ProductId, cancellationToken);
            if(companyStock.Stock < request.Model.Amount)
                return new ApiResponse<OrderItemResponse>("Supplier stock not enough.");
                
            // Update Supplier Stock and Cart
            decimal ProfitMargin = (company.Dealer.ProfitMargin + 100) / 100;
            companyStock.Stock +=orderItem.Amount - request.Model.Amount ;

            orderItem.Order.UnitPriceSum += orderItem.Product.UnitPrice * ProfitMargin * (request.Model.Amount - orderItem.Amount);
            orderItem.Order.KDVPriceSum += orderItem.Product.KDV / 100 * orderItem.Product.UnitPrice * ProfitMargin * (request.Model.Amount - orderItem.Amount);
            await dbContext.SaveChangesAsync(cancellationToken);
            orderItem.Order.TotalPrice = orderItem.Order.UnitPriceSum + orderItem.Order.KDVPriceSum;

            // Update
            orderItem.Amount = request.Model.Amount;
            await dbContext.SaveChangesAsync(cancellationToken);
            
            // Response
            var response = mapper.Map<OrderItemResponse>(orderItem);
            return new ApiResponse<OrderItemResponse>(response);
        }

        public async Task<ApiResponse> Handle(DeleteAllOrderItemCommand request, CancellationToken cancellationToken)
        {
            // Check
            Order order = await dbContext.Set<Order>().FirstOrDefaultAsync(x => x.CompanyId == request.CompanyId && x.Status.Equals("Cart"), cancellationToken);
            List<OrderItem> orderItems = await dbContext.Set<OrderItem>().Include(x => x.Order)
                .Where(x => x.OrderId == order.Id).ToListAsync(cancellationToken);
            if(orderItems == null)
                return new ApiResponse("Order not found.");
            
            // Delete
            unitOfWork.OrderItemRepository.RemoveRange(orderItems);
            
            // Update Supplier Stock and Cart
            var company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Dealer");
            foreach (var orderItem in orderItems)
            {
                CompanyStock companyStock = await dbContext.Set<CompanyStock>().FirstOrDefaultAsync(x => x.CompanyId == company.Dealer.SupplierId && x.ProductId == orderItem.ProductId, cancellationToken);
                companyStock.Stock += orderItem.Amount;
            }
            order.UnitPriceSum = 0;
            order.KDVPriceSum = 0;
            order.TotalPrice = (decimal)0;
            await dbContext.SaveChangesAsync(cancellationToken);

            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            // Check
            OrderItem orderItem = await unitOfWork.OrderItemRepository.GetById(cancellationToken, request.OrderItemId, "Product", "Order");
            if(orderItem == null)
                return new ApiResponse("Order not found.");

            // Update Supplier Stock and Cart
            var company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Dealer");
            CompanyStock companyStock = await dbContext.Set<CompanyStock>().FirstOrDefaultAsync(x => x.CompanyId == company.Dealer.SupplierId && x.ProductId == orderItem.ProductId, cancellationToken);
            companyStock.Stock += orderItem.Amount;

            decimal ProfitMargin = (company.Dealer.ProfitMargin + 100) / 100;

            orderItem.Order.UnitPriceSum -= orderItem.Product.UnitPrice * ProfitMargin * orderItem.Amount;
            orderItem.Order.KDVPriceSum -= orderItem.Product.KDV / 100 * orderItem.Product.UnitPrice * ProfitMargin * orderItem.Amount;
            await dbContext.SaveChangesAsync(cancellationToken);
            orderItem.Order.TotalPrice = orderItem.Order.UnitPriceSum + orderItem.Order.KDVPriceSum;
            await dbContext.SaveChangesAsync(cancellationToken);
            
            // Delete
            unitOfWork.OrderItemRepository.Remove(orderItem);
            
            return new ApiResponse();
        }
    }
}