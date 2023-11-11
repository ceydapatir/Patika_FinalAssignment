using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.PaymentCqrs;

namespace DealerManagement.Operation.Query
{
    public class PaymentQueryHandler :
        IRequestHandler<GetPaymentByOrderIdQuery, ApiResponse<PaymentResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public PaymentQueryHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<PaymentResponse>> Handle(GetPaymentByOrderIdQuery request, CancellationToken cancellationToken)
        {
            // Check
            Payment payment = await dbContext.Set<Payment>().FirstOrDefaultAsync( x => x.OrderId == request.OrderId, cancellationToken);
            if(payment == null)
                return new ApiResponse<PaymentResponse>("Payment not found.");
            
            // Response
            PaymentResponse mapped = mapper.Map<PaymentResponse>(payment);
            return new ApiResponse<PaymentResponse>(mapped);
        }
    }
}