
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.SupplierCqrs;

namespace DealerManagement.Operation.Query
{
    public class SupplierQueryHandler :
        IRequestHandler<GetSupplierQuery, ApiResponse<SupplierResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public SupplierQueryHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<SupplierResponse>> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
        {   
            int companyId;
            if(request.EmployeeRole.Equals("admin")){
                companyId = request.CompanyId;
            }else{
                var dealer_company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Dealer");
                companyId = dealer_company.Dealer.SupplierId;
            }
            // Check
            Company company = await dbContext.Set<Company>().Include(x => x.Supplier).FirstOrDefaultAsync(x => x.Supplier.Id == companyId, cancellationToken);
            if(company == null)
                return new ApiResponse<SupplierResponse>("Supplier not found.");

            // Response
            SupplierResponse mapped = mapper.Map<SupplierResponse>(company);
            return new ApiResponse<SupplierResponse>(mapped);
        }

    }
}