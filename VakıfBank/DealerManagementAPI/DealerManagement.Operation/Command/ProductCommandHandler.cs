
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Operation.Validation;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.ProductCqrs;

namespace DealerManagement.Operation.Command
{
    public class ProductCommandHandler :
        IRequestHandler<CreateProductCommand, ApiResponse<ProductResponse>>,
        IRequestHandler<UpdateProductCommand, ApiResponse<ProductResponse>>,
        IRequestHandler<DeleteProductCommand, ApiResponse>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ProductCommandHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Validation
            ProductValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<ProductResponse>(result.Errors[0].ToString());

            // Check
            Product product = await dbContext.Set<Product>().FirstOrDefaultAsync(x => x.ProductCode == request.Model.ProductCode, cancellationToken);
            if(product is not null)
                return new ApiResponse<ProductResponse>("Product already exist.");

            // Create
            Product mapped = mapper.Map<Product>(request.Model);
            var product_id = await unitOfWork.ProductRepository.InsertAndGetId(cancellationToken, mapped);

            CompanyStock companyStock = new CompanyStock(){ CompanyId = request.CompanyId, ProductId = product_id, Stock = request.Model.Stock};
            unitOfWork.CompanyStockRepository.Insert(companyStock); // add the product to the company stock

            // Response
            var response = mapper.Map<ProductResponse>(companyStock);
            return new ApiResponse<ProductResponse>(response);
        }

        public async Task<ApiResponse<ProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // Validation
            ProductUpdateValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<ProductResponse>(result.Errors[0].ToString());

            // Check
            CompanyStock companyStock = await dbContext.Set<CompanyStock>().Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.ProductId == request.ProductId && x.CompanyId == request.CompanyId, cancellationToken);
            if(companyStock == null)
                return new ApiResponse<ProductResponse>("Product not found.");
            
            // Update
            companyStock.Product.ProductName = !request.Model.ProductName.Equals("") && (request.Model.ProductName is not null) ? request.Model.ProductName : companyStock.Product.ProductName;
            companyStock.Product.UnitPrice = !request.Model.UnitPrice.Equals(0) ? (decimal)request.Model.UnitPrice : companyStock.Product.UnitPrice;
            companyStock.Stock = !request.Model.Stock.Equals(0) ? request.Model.Stock : companyStock.Stock;
            await dbContext.SaveChangesAsync(cancellationToken);

            // Response
            var response = mapper.Map<ProductResponse>(companyStock);
            return new ApiResponse<ProductResponse>(response);
        }

        public async Task<ApiResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // Check
            var product = await unitOfWork.ProductRepository.GetById(cancellationToken, request.ProductId, "CompanyStocks");
            if (product == null)
                return new ApiResponse("Product not found.");

            // Delete
            unitOfWork.CompanyStockRepository.RemoveRange(product.CompanyStocks);
            unitOfWork.ProductRepository.Remove(request.ProductId);
            
            return new ApiResponse();
        }
    }
}