
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class TokenCqrs
    {
        //Commands
        public record CreateTokenCommand(TokenRequest Model) : IRequest<ApiResponse<TokenResponse>>;
    }
}