using System.Security.Claims;
using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DealerManagement.Operation.Cqrs.ProductCqrs;

namespace DealerManagement.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private IMediator mediator;

        public ProductsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<List<ProductResponse>>> GetOwnCompanyProducts(){ 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new GetAllProductsQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("/api/dealers/{company_id}/products")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<ProductResponse>>> GetDealerProductsByDealerId(){ 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new GetDealerProductsQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("/api/supplier/products")]
        [Authorize(Roles = "dealer")]
        public async Task<ApiResponse<List<ProductResponse>>> GetSupplierProducts(){ 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new GetSupplierProductsQuery(company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("{product_id}")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<ProductResponse>> GetProductById([FromRoute] int product_id) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new GetProductByIdQuery(company_id, product_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductResponse>> CreateProduct([FromBody] ProductRequest model) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new CreateProductCommand(model, company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("{product_id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductResponse>> UpdateProductStock([FromRoute] int product_id, [FromBody] ProductUpdateRequest model) { 
            var company_id = int.Parse((User.Identity as ClaimsIdentity).FindFirst("CompanyId").Value);
            var operation = new UpdateProductCommand(model, product_id, company_id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{product_id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse>  DeleteProduct([FromRoute] int product_id) {
            var operation = new DeleteProductCommand(product_id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}