using DealerManagement.Base.Response;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;
using MediatR;

namespace DealerManagement.Operation.Cqrs
{
    public class AddressCqrs
    {
        //Commands
        public record CreateAddressCommand(AddressRequest Model, int CompanyId) : IRequest<ApiResponse<AddressResponse>>;
        public record UpdateAddressCommand(AddressRequest Model, int CompanyId) : IRequest<ApiResponse<AddressResponse>>;

        //Queries
        public record GetAddressQuery(int CompanyId) : IRequest<ApiResponse<AddressResponse>>;
        
    }
}