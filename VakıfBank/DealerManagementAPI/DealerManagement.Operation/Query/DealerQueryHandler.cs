using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.DealerCqrs;

namespace DealerManagement.Operation.Query
{
    public class DealerQueryHandler :
        IRequestHandler<GetCompanyQuery, ApiResponse<DealerResponse>>,
        IRequestHandler<GetAllDealersQuery, ApiResponse<List<DealerResponse>>>,
        IRequestHandler<GetDealerByIdQuery, ApiResponse<DealerResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public DealerQueryHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<DealerResponse>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            // Check
            var company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Dealer");
            if(company == null)
                return new ApiResponse<DealerResponse>("Dealer not found.");
            
            // Response
            DealerResponse mapped = mapper.Map<DealerResponse>(company);
            return new ApiResponse<DealerResponse>(mapped);   
        }

        public async Task<ApiResponse<List<DealerResponse>>> Handle(GetAllDealersQuery request, CancellationToken cancellationToken)
        {   
            // Check
            List<Company> companies = await dbContext.Set<Company>().Include(x => x.Dealer).Where(x => x.Dealer.SupplierId == request.CompanyId).ToListAsync(cancellationToken);
            if(companies.Count() == 0)
                return new ApiResponse<List<DealerResponse>>("There are no dealers.");

            // Response
            List<DealerResponse> mapped = mapper.Map<List<DealerResponse>>(companies);
            return new ApiResponse<List<DealerResponse>>(mapped);
        }

        public async Task<ApiResponse<DealerResponse>> Handle(GetDealerByIdQuery request, CancellationToken cancellationToken)
        {
            // Check
            Company company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Dealer");
            if(company == null)
                return new ApiResponse<DealerResponse>("Dealer not found.");
            
            // Response
            DealerResponse mapped = mapper.Map<DealerResponse>(company);
            return new ApiResponse<DealerResponse>(mapped);
        }
    }
}