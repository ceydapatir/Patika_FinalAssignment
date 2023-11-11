
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class CardCqrs
    {
        //Commands
        public record CreateCardCommand(CardRequest Model, int CompanyId) : IRequest<ApiResponse<CardResponse>>;
        public record DeleteCardCommand(int CardId) : IRequest<ApiResponse>;

        //Queries
        public record GetAllCardsQuery(int CompanyId) : IRequest<ApiResponse<List<CardResponse>>>;
        public record GetCardByIdQuery(int CardId) : IRequest<ApiResponse<CardResponse>>;
    }
}