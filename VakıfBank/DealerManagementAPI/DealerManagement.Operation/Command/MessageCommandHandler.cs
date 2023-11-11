using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Operation.Validation;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.MessageCqrs;

namespace DealerManagement.Operation.Command
{
    public class MessageCommandHandler:
        IRequestHandler<CreateMessageCommand, ApiResponse<MessageResponse>>,
        IRequestHandler<DeleteMessageCommand, ApiResponse>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public MessageCommandHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<MessageResponse>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            // Validation
            MessageValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<MessageResponse>(result.Errors[0].ToString());

            // Check
            Message message = await dbContext.Set<Message>().Include(x => x.MessageContents)
                .FirstOrDefaultAsync(x => x.SenderId == request.EmployeeId && x.ReceiverId == request.Model.ReceiverId, cancellationToken);
                // if null create new chat
            if(message == null){
                message = new Message(){ SenderId = request.EmployeeId, ReceiverId = request.Model.ReceiverId};
                message = await unitOfWork.MessageRepository.InsertAndGetEntity(cancellationToken, message);
            }

            // Create
            MessageContent message_content = new MessageContent(){ MessageId = message.Id, Content = request.Model.Content};
            unitOfWork.MessageContentRepository.Insert(message_content);

            // Response
            var response = mapper.Map<MessageResponse>(message_content);
            return new ApiResponse<MessageResponse>(response);
        }

        public async Task<ApiResponse> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            // Check
            Message message = await unitOfWork.MessageRepository.GetById(cancellationToken, request.MessageId, "MessageContents");
            if(message == null)
                return new ApiResponse("Order not found.");

            // Delete
            unitOfWork.MessageContentRepository.RemoveRange(message.MessageContents);
            unitOfWork.MessageRepository.Remove(message);

            return new ApiResponse();
        }
    }
}