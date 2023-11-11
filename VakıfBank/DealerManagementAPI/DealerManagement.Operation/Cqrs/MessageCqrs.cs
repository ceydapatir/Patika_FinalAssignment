using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class MessageCqrs
    {
        //Commands
        public record CreateMessageCommand(MessageRequest Model, int EmployeeId) : IRequest<ApiResponse<MessageResponse>>;
        public record DeleteMessageCommand(int MessageId) : IRequest<ApiResponse>;

        //Queries
        public record GetAllMessagesQuery(int EmployeeId) : IRequest<ApiResponse<List<MessageResponse>>>;   
    }
}