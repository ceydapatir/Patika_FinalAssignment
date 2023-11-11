
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Operation.Validation;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.CardCqrs;

namespace DealerManagement.Operation.Command
{
    public class CardCommandHandler :
        IRequestHandler<CreateCardCommand, ApiResponse<CardResponse>>,
        IRequestHandler<DeleteCardCommand, ApiResponse>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CardCommandHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<CardResponse>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            // Validation
            CardValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<CardResponse>(result.Errors[0].ToString());

            // Check
            CompanyCard companyCard = await dbContext.Set<CompanyCard>().FirstOrDefaultAsync(x => x.CardNumber.Equals(request.Model.CardNumber) , cancellationToken);
            if(companyCard is not null)
                return new ApiResponse<CardResponse>("Card already exist.");

            Card card = await dbContext.Set<Card>().Include(x => x.Account).FirstOrDefaultAsync(x => x.CardNumber.Equals(request.Model.CardNumber) && x.CVV.Equals(request.Model.CVV) && x.ExpireDate.Equals(request.Model.ExpireDate) , cancellationToken);
            if(card == null)
                return new ApiResponse<CardResponse>("Card not found.");
            
            // Create
            if( card.Account.CheckingAccount == null){
                companyCard = new CompanyCard(){ CardName = request.Model.CardName, CardNumber = request.Model.CardNumber, CompanyId = request.CompanyId, CardType = "deposit"};
            }else{
                CompanyCard mapped = new CompanyCard(){ CardName = request.Model.CardName, CardNumber = request.Model.CardNumber, CompanyId = request.CompanyId, CardType = "credit"};
            }
            unitOfWork.CompanyCardRepository.Insert(companyCard);
            
            // Response
            var response = mapper.Map<CardResponse>(companyCard);
            return new ApiResponse<CardResponse>(response);
        }

        public async Task<ApiResponse> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
        {         
            // Check
            var card = await unitOfWork.CompanyCardRepository.GetById(cancellationToken, request.CardId);
            if(card == null)
                return new ApiResponse("Card not found.");

            // Delete
            unitOfWork.CompanyCardRepository.Remove(request.CardId);
            return new ApiResponse();
        }

    }
}