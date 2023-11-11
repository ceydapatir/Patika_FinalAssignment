
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class ProductCqrs
    {
        //Commands
        public record CreateProductCommand(ProductRequest Model, int CompanyId) : IRequest<ApiResponse<ProductResponse>>;
        public record UpdateProductCommand(ProductUpdateRequest Model, int ProductId, int CompanyId) : IRequest<ApiResponse<ProductResponse>>;
        public record DeleteProductCommand(int ProductId) : IRequest<ApiResponse>;

        //Queries
        public record GetAllProductsQuery(int CompanyId) : IRequest<ApiResponse<List<ProductResponse>>>;
        public record GetDealerProductsQuery(int CompanyId) : IRequest<ApiResponse<List<ProductResponse>>>;
        public record GetSupplierProductsQuery(int CompanyId) : IRequest<ApiResponse<List<ProductResponse>>>;
        public record GetProductByIdQuery(int CompanyId, int ProductId) : IRequest<ApiResponse<ProductResponse>>;
    }
}