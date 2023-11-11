
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Operation.Validation;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.PaymentCqrs;

namespace DealerManagement.Operation.Command
{
    public class PaymentCommandHandler : 
        IRequestHandler<CreatePaymentCommand, ApiResponse<PaymentResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public PaymentCommandHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<PaymentResponse>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            // Validation
            PaymentValidator validator = new(dbContext, request.CompanyId);
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<PaymentResponse>(result.Errors[0].ToString());

            // Check
            Order order = await dbContext.Set<Order>().FirstOrDefaultAsync(x => x.CompanyId == request.CompanyId && x.Status.Equals("Cart"), cancellationToken);
            if(order == null)
                    return new ApiResponse<PaymentResponse>("Cart not found.");
            if(order.TotalPrice == 0)
                    return new ApiResponse<PaymentResponse>("Cart is empty.");
            
            // Create
            Payment payment = new();
            if(request.Model.PaymentType.Equals("eft_transfer")){
                payment = new Payment(){ CardId = null, OrderId = order.Id, PaymentType = request.Model.PaymentType};
                order.Status = "Confirmation Awaited";
                await dbContext.SaveChangesAsync(cancellationToken);
                unitOfWork.PaymentRepository.Insert(payment);
            }else{
                var companyCard = await unitOfWork.CompanyCardRepository.GetById(cancellationToken, request.Model.CardId);
                Card card = await dbContext.Set<Card>().FirstOrDefaultAsync(x => x.CardNumber.Equals(companyCard.CardNumber), cancellationToken);
                payment = new Payment(){ CardId = request.Model.CardId, OrderId = order.Id, PaymentType = request.Model.PaymentType};
                
                // Budget control 
                if(companyCard.CardType.Equals("credit")){
                    if(card.ExpenseLimit >= order.TotalPrice){
                        order.Status = "Confirmation Awaited";
                        card.ExpenseLimit -= order.TotalPrice;
                        await dbContext.SaveChangesAsync(cancellationToken);
                        unitOfWork.PaymentRepository.Insert(payment);
                    }else{
                        return new ApiResponse<PaymentResponse>("Inadequate limit.");
                    }
                }else if(companyCard.CardType.Equals("deposit")){
                    DepositAccount account = await dbContext.Set<DepositAccount>().FirstOrDefaultAsync(x => x.AccountId == card.AccountId, cancellationToken);
                    if((account.OpeningAmount + account.DebtTotal) >= order.TotalPrice){
                        order.Status = "Confirmation Awaited";
                        account.DebtTotal -= order.TotalPrice;
                        await dbContext.SaveChangesAsync(cancellationToken);
                        unitOfWork.PaymentRepository.Insert(payment);
                    }else{                    
                        return new ApiResponse<PaymentResponse>("Inadequate limit.");
                    }
                }
            }

            // new cart
            Dealer dealer = await dbContext.Set<Dealer>().Include(x => x.Supplier).FirstOrDefaultAsync(x => x.CompanyId == request.CompanyId, cancellationToken);
            Order neworder = new Order(){ CompanyId = request.CompanyId, SupplierIBAN = dealer.Supplier.IBAN};
            unitOfWork.OrderRepository.Insert(neworder);

            // Response
            var response = mapper.Map<PaymentResponse>(payment);
            return new ApiResponse<PaymentResponse>(response);
        }
    }
}