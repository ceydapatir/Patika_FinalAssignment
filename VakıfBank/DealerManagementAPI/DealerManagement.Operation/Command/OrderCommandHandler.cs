
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.OrderCqrs;

namespace DealerManagement.Operation.Command
{
    public class OrderCommandHandler :
        IRequestHandler<UpdateOrderCommand, ApiResponse<OrderResponse>>,
        IRequestHandler<DeleteOrderCommand, ApiResponse<OrderResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public OrderCommandHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<OrderResponse>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            // Check
            Order order = await unitOfWork.OrderRepository.GetById(cancellationToken, request.OrderId, "Payment");
            if(order == null)
                return new ApiResponse<OrderResponse>("Order not found.");
            if(order.Status.Equals("Confirmed"))
                return new ApiResponse<OrderResponse>("The order is already confirmed.");
            if(order.Status.Equals("Cancelled") || order.Status.Equals("Cart"))
                return new ApiResponse<OrderResponse>("The order cannot be confirmed.");

            // Update
            order.Status = "Confirmed";
            order.BillingCode = order.CompanyId.ToString()+order.Id.ToString();
            await dbContext.SaveChangesAsync(cancellationToken);

            // If payment is made by credit card or open account, the payment process is completed.
            Payment payment = await dbContext.Set<Payment>().FirstOrDefaultAsync(x => x.OrderId == order.Id, cancellationToken);
            if(payment.CardId is not null){
                Account account = await dbContext.Set<Account>().Include(x => x.CheckingAccount).FirstOrDefaultAsync(x => x.IBAN.Equals(order.SupplierIBAN), cancellationToken);
                account.CheckingAccount.Balance += order.TotalPrice;
                payment.Status = "Paid";
                payment.PaymentDate = DateTime.Now;
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            // Add orderItems to Dealer stock
            List<OrderItem> orderItems = await dbContext.Set<OrderItem>().Where(x => x.OrderId == order.Id).ToListAsync(cancellationToken);
            foreach (var orderItem in orderItems)
            {
                CompanyStock companyStock = await dbContext.Set<CompanyStock>().FirstOrDefaultAsync(x => x.CompanyId == order.CompanyId && x.ProductId == orderItem.ProductId, cancellationToken);
                if(companyStock is not null){
                    companyStock.Stock += orderItem.Amount;
                    await dbContext.SaveChangesAsync(cancellationToken);
                }else{
                    CompanyStock dealerStock = new CompanyStock(){ CompanyId = order.CompanyId, ProductId = orderItem.ProductId, Stock = orderItem.Amount};
                    unitOfWork.CompanyStockRepository.Insert(dealerStock);
                }
            }

            // Response
            var response = mapper.Map<OrderResponse>(order);
            return new ApiResponse<OrderResponse>(response);
        }
        public async Task<ApiResponse<OrderResponse>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            // Check
            Order order = await unitOfWork.OrderRepository.GetById(cancellationToken, request.OrderId);
            if(order == null)
                return new ApiResponse<OrderResponse>("Order not found.");
            if(order.Status.Equals("Confirmed") || order.Status.Equals("Cart"))
                return new ApiResponse<OrderResponse>("The order cannot be canceled.");

            // Update
            order.Status = "Cancelled";
            await dbContext.SaveChangesAsync(cancellationToken);

            // Refund to card
            Payment payment = await dbContext.Set<Payment>().Include(x => x.Card).FirstOrDefaultAsync(x => x.OrderId == order.Id, cancellationToken);
            if(payment.Card is not null){
                Card card = await dbContext.Set<Card>().FirstOrDefaultAsync(x => x.CardNumber.Equals(payment.Card.CardNumber), cancellationToken);
                if(payment.Card.CardType.Equals("credit")){
                    card.ExpenseLimit += order.TotalPrice;
                }else if(payment.Card.CardType.Equals("deposit")){
                    DepositAccount account = await dbContext.Set<DepositAccount>().FirstOrDefaultAsync(x => x.AccountId == card.AccountId, cancellationToken);
                    account.DebtTotal += order.TotalPrice;
                }
            }
            payment.Status = "Cancelled";
            await dbContext.SaveChangesAsync(cancellationToken);

            // Stock return
            List<OrderItem> orderItems = await dbContext.Set<OrderItem>().Where(x => x.OrderId == order.Id).ToListAsync(cancellationToken);
            foreach (var orderItem in orderItems)
            {
                CompanyStock companyStock = await dbContext.Set<CompanyStock>().FirstOrDefaultAsync(x => x.CompanyId == request.CompanyId && x.ProductId == orderItem.ProductId, cancellationToken);
                companyStock.Stock += orderItem.Amount;
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            // Response
            var response = mapper.Map<OrderResponse>(order);
            return new ApiResponse<OrderResponse>(response);
        }

    }
}