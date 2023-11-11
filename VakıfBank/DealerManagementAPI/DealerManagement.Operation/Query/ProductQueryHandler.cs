
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.ProductCqrs;

namespace DealerManagement.Operation.Query
{
    public class ProductQueryHandler :
        IRequestHandler<GetAllProductsQuery, ApiResponse<List<ProductResponse>>>,
        IRequestHandler<GetDealerProductsQuery, ApiResponse<List<ProductResponse>>>,
        IRequestHandler<GetSupplierProductsQuery, ApiResponse<List<ProductResponse>>>,
        IRequestHandler<GetProductByIdQuery, ApiResponse<ProductResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ProductQueryHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<ProductResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // Check
            List<CompanyStock> companyStocks = await dbContext.Set<CompanyStock>().Include(x => x.Product).Where(x => x.CompanyId == request.CompanyId).ToListAsync(cancellationToken);
            if(companyStocks.Count() == 0)
                return new ApiResponse<List<ProductResponse>>("Product not found.");

            // Response
            List<ProductResponse> mapped = mapper.Map<List<ProductResponse>>(companyStocks);
            return new ApiResponse<List<ProductResponse>>(mapped);
        }

        public async Task<ApiResponse<List<ProductResponse>>> Handle(GetDealerProductsQuery request, CancellationToken cancellationToken)
        {
            // Check
            List<CompanyStock> companyStocks = new();
            Supplier supplier = await dbContext.Set<Supplier>().Include(x => x.Dealers).FirstOrDefaultAsync(x => x.CompanyId == request.CompanyId, cancellationToken);
            foreach (var dealer in supplier.Dealers)
            {
                List<CompanyStock> dealerStocks = await dbContext.Set<CompanyStock>().Include(x => x.Product).Where(x => x.CompanyId == dealer.CompanyId).ToListAsync(cancellationToken);
                companyStocks.AddRange(dealerStocks);
            }
            if(companyStocks.Count() == 0)
                return new ApiResponse<List<ProductResponse>>("Product not found.");

            // Response
            List<ProductResponse> mapped = mapper.Map<List<ProductResponse>>(companyStocks);
            return new ApiResponse<List<ProductResponse>>(mapped);
        }

        public async Task<ApiResponse<List<ProductResponse>>> Handle(GetSupplierProductsQuery request, CancellationToken cancellationToken)
        {
            // Check
            Dealer dealer = await dbContext.Set<Dealer>().Include(x => x.Supplier).FirstOrDefaultAsync(x => x.CompanyId == request.CompanyId, cancellationToken);
            List<CompanyStock> companyStocks = await dbContext.Set<CompanyStock>().Include(x => x.Product).Where(x => x.CompanyId == dealer.Supplier.CompanyId).ToListAsync(cancellationToken);
            if(companyStocks.Count() == 0)
                return new ApiResponse<List<ProductResponse>>("Product not found.");
            
            // Response after adding ProfitMargin
            List<ProductResponse> mapped = mapper.Map<List<ProductResponse>>(companyStocks);
            foreach (var mapped_item in mapped)
            {
                mapped_item.UnitPrice = mapped_item.UnitPrice * (double)dealer.ProfitMargin / 100;
            }
            return new ApiResponse<List<ProductResponse>>(mapped);
        }

        public async Task<ApiResponse<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Check
            CompanyStock companyStock = await dbContext.Set<CompanyStock>().Include(x => x.Product).FirstOrDefaultAsync(x => x.CompanyId == request.CompanyId && x.ProductId == request.ProductId, cancellationToken);
            if(companyStock == null)
                return new ApiResponse<ProductResponse>("Product not found.");
            
            // Response
            ProductResponse mapped = mapper.Map<ProductResponse>(companyStock);
            return new ApiResponse<ProductResponse>(mapped);
        }
    }
}