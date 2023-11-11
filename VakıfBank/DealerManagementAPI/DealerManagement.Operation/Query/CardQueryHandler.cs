using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.CardCqrs;

namespace DealerManagement.Operation.Query
{
    public class CardQueryHandler :
        IRequestHandler<GetAllCardsQuery, ApiResponse<List<CardResponse>>>,
        IRequestHandler<GetCardByIdQuery, ApiResponse<CardResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CardQueryHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<CardResponse>>> Handle(GetAllCardsQuery request, CancellationToken cancellationToken)
        {
            // Check
            List<CompanyCard> cards = await dbContext.Set<CompanyCard>().Where(x => x.CompanyId == request.CompanyId).ToListAsync(cancellationToken);
            if(cards.Count() == 0)
                return new ApiResponse<List<CardResponse>>("Card not found.");

            // Response
            List<CardResponse> mapped = mapper.Map<List<CardResponse>>(cards);
            return new ApiResponse<List<CardResponse>>(mapped);
        }

        public async Task<ApiResponse<CardResponse>> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
        {
            // Check
            CompanyCard card = await unitOfWork.CompanyCardRepository.GetById(cancellationToken, request.CardId);
            if(card == null)
                return new ApiResponse<CardResponse>("Card not found.");
            
            // Response
            CardResponse mapped = mapper.Map<CardResponse>(card);
            return new ApiResponse<CardResponse>(mapped);
        }
    }
}