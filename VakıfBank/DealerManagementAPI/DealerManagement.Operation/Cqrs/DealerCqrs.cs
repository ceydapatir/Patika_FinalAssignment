using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class DealerCqrs
    {
        //Commands
        public record CreateDealerCommand(DealerRequest Model, int CompanyId) : IRequest<ApiResponse<DealerResponse>>;
        public record UpdateProfitMarginCommand(UpdateProfitMarginRequest Model, int CompanyId) : IRequest<ApiResponse<DealerResponse>>;
        public record UpdateDealerCommand(DealerUpdateRequest Model, int CompanyId) : IRequest<ApiResponse<DealerResponse>>;
        public record DeleteDealerCommand(int CompanyId) : IRequest<ApiResponse>;

        //Queries
        public record GetCompanyQuery(int CompanyId) : IRequest<ApiResponse<DealerResponse>>;
        public record GetAllDealersQuery(int CompanyId) : IRequest<ApiResponse<List<DealerResponse>>>;
        public record GetDealerByIdQuery(int CompanyId) : IRequest<ApiResponse<DealerResponse>>;    
    }
}