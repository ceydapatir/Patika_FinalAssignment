
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Operation.Validation;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.EmployeeCqrs;

namespace DealerManagement.Operation.Command
{
    public class EmployeeCommandHandler :
        IRequestHandler<CreateEmployeeCommand, ApiResponse<EmployeeResponse>>,
        IRequestHandler<UpdateEmployeeCommand, ApiResponse<EmployeeResponse>>,
        IRequestHandler<DeleteEmployeeCommand, ApiResponse>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public EmployeeCommandHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<EmployeeResponse>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Validation
            EmployeeValidator validator = new(dbContext);
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<EmployeeResponse>(result.Errors[0].ToString());

            // Check
            Employee employee = await dbContext.Set<Employee>().FirstOrDefaultAsync(x => x.EmployeeNumber.Equals(request.Model.EmployeeNumber), cancellationToken);
            if(employee is not null)
                return new ApiResponse<EmployeeResponse>("Employee already exist.");
            
            // Create
            Employee mapped = mapper.Map<Employee>(request.Model);
            unitOfWork.EmployeeRepository.Insert(mapped);

            // Response
            var response = mapper.Map<EmployeeResponse>(mapped);
            return new ApiResponse<EmployeeResponse>(response);
        }

        public async Task<ApiResponse<EmployeeResponse>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Validation
            EmployeeUpdateValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<EmployeeResponse>(result.Errors[0].ToString());

            // Check
            var employee = await unitOfWork.EmployeeRepository.GetById(cancellationToken, request.EmployeeId);
            if(employee == null)
                return new ApiResponse<EmployeeResponse>("Employee not found.");
            
            // Update
            employee.Password = !request.Model.Password.Equals("") && (request.Model.Password is not null) ? request.Model.Password : employee.Password;
            employee.Mail = !request.Model.Mail.Equals("") && (request.Model.Mail is not null ) ? request.Model.Mail : employee.Mail;
            await dbContext.SaveChangesAsync(cancellationToken);

            // Response
            var response = mapper.Map<EmployeeResponse>(employee);
            return new ApiResponse<EmployeeResponse>(response);
        }

        public async Task<ApiResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {   
            // Check
            var employee = await unitOfWork.EmployeeRepository.GetById(cancellationToken, request.EmployeeId);
            if(employee == null)
                return new ApiResponse("Employee not found."); 

            // Delete
            unitOfWork.EmployeeRepository.Remove(request.EmployeeId);
            
            return new ApiResponse();
        }
    }
}