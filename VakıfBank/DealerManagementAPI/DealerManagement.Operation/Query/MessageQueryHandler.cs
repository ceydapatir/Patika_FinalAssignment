
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.MessageCqrs;

namespace DealerManagement.Operation.Query
{
    public class MessageQueryHandler:
        IRequestHandler<GetAllMessagesQuery, ApiResponse<List<MessageResponse>>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public MessageQueryHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<MessageResponse>>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
        {
            // Check
            List<MessageContent> messages = await dbContext.Set<MessageContent>().Include(x => x.Message).Where(x => x.Message.SenderId == request.EmployeeId || x.Message.ReceiverId == request.EmployeeId).ToListAsync(cancellationToken);
            if(messages.Count() == 0)
                return new ApiResponse<List<MessageResponse>>("Message not found.");
            
            // Response
            List<MessageResponse> mapped = mapper.Map<List<MessageResponse>>(messages);
            return new ApiResponse<List<MessageResponse>>(mapped);
        }
    }
}