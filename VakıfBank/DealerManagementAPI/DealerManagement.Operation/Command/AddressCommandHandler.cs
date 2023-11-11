
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Operation.Validation;
using DealerManagement.Schema.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.AddressCqrs;

namespace DealerManagement.Operation.Command
{
    public class AddressCommandHandler :
        IRequestHandler<CreateAddressCommand, ApiResponse<AddressResponse>>,
        IRequestHandler<UpdateAddressCommand, ApiResponse<AddressResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public AddressCommandHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<AddressResponse>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            // Validation
            AddressValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<AddressResponse>(result.Errors[0].ToString());
            
            // Check
            var company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Address");
            if(company.CompanyAddress is not null)
                return new ApiResponse<AddressResponse>("Company already has an address.");

            // Create
            var mapped = mapper.Map<Address>(request.Model);
            mapped.CompanyId = request.CompanyId;
            unitOfWork.AddressRepository.Insert(mapped);

            // Response
            var response = mapper.Map<AddressResponse>(mapped);
            return new ApiResponse<AddressResponse>(response);
        }

        public async Task<ApiResponse<AddressResponse>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {   
            // Validation
            AddressUpdateValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<AddressResponse>(result.Errors[0].ToString());
            
            // Check
            Address address = await dbContext.Set<Address>().FirstOrDefaultAsync(x => x.CompanyId == request.CompanyId, cancellationToken);
            if(address == null)
                return new ApiResponse<AddressResponse>("Address not found.");
            
            // Update
            address.Country = !request.Model.Country.Equals("") && (request.Model.Country is not null) ? request.Model.Country : address.Country;
            address.City = !request.Model.City.Equals("") && (request.Model.City is not null) ? request.Model.City : address.City;
            address.Province = !request.Model.Province.Equals("") && (request.Model.Province is not null) ? request.Model.Province : address.Province;
            address.AddressLine1 = !request.Model.AddressLine1.Equals("") && (request.Model.AddressLine1 is not null) ? request.Model.AddressLine1 : address.AddressLine1;
            address.AddressLine2 = !request.Model.AddressLine2.Equals("") && (request.Model.AddressLine2 is not null) ? request.Model.AddressLine2 : address.AddressLine2;
            address.PostalCode = !request.Model.PostalCode.Equals("") && (request.Model.PostalCode is not null) ? request.Model.PostalCode : address.PostalCode;
            await dbContext.SaveChangesAsync(cancellationToken);

            // Response
            var response = mapper.Map<AddressResponse>(address);
            return new ApiResponse<AddressResponse>(response);
        }
    }
}