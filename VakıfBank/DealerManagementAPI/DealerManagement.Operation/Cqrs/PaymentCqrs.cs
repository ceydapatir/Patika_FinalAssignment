
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class PaymentCqrs
    {
        //Commands
        public record CreatePaymentCommand(PaymentRequest Model, int CompanyId) : IRequest<ApiResponse<PaymentResponse>>;

        //Queries
        public record GetPaymentByOrderIdQuery(int OrderId) : IRequest<ApiResponse<PaymentResponse>>;
    }
}