
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.UoW;
using DealerManagement.Operation.Validation;
using DealerManagement.Schema.Responses;
using MediatR;
using static DealerManagement.Operation.Cqrs.SupplierCqrs;

namespace DealerManagement.Operation.Command
{
    public class SupplierCommandHandler :
        IRequestHandler<UpdateSupplierCommand, ApiResponse<SupplierResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public SupplierCommandHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<SupplierResponse>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            // Validation
            SupplierUpdateValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<SupplierResponse>(result.Errors[0].ToString());

            // Check
            var company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Supplier");
            if(company == null)
                return new ApiResponse<SupplierResponse>("Company not found.");
            
            // Update
            company.Mail = !request.Model.Mail.Equals("") && (request.Model.Mail is not null ) ? request.Model.Mail : company.Mail;
            await dbContext.SaveChangesAsync(cancellationToken);

            // Response
            var response = mapper.Map<SupplierResponse>(company);
            return new ApiResponse<SupplierResponse>(response);
        }
    }
}