using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.AddressCqrs;

namespace DealerManagement.Operation.Query
{
    public class AddressQueryHandler :
        IRequestHandler<GetAddressQuery, ApiResponse<AddressResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public AddressQueryHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<AddressResponse>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
        {
            // Check
            Address address = await dbContext.Set<Address>().FirstOrDefaultAsync(x => x.Company.Id == request.CompanyId, cancellationToken);
            if(address == null)
                return new ApiResponse<AddressResponse>("Address not found.");
            
            // Response
            AddressResponse mapped = mapper.Map<AddressResponse>(address);
            return new ApiResponse<AddressResponse>(mapped);
        }
    }
}