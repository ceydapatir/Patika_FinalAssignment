using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.EmployeeCqrs;

namespace DealerManagement.Operation.Query
{
    public class EmployeeQueryHandler :
        IRequestHandler<GetEmployeesByCompanyIdQuery, ApiResponse<List<EmployeeResponse>>>,
        IRequestHandler<GetEmployeeByIdQuery, ApiResponse<EmployeeResponse>>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public EmployeeQueryHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ApiResponse<List<EmployeeResponse>>> Handle(GetEmployeesByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            // Check         
            List<Employee> employees = await dbContext.Set<Employee>().Include(x => x.Company).Where(x => x.CompanyId == request.CompanyId).ToListAsync(cancellationToken);
            if(employees.Count() == 0)
                return new ApiResponse<List<EmployeeResponse>>("Employee not found.");
            
            // Response
            List<EmployeeResponse> mapped = mapper.Map<List<EmployeeResponse>>(employees);
            return new ApiResponse<List<EmployeeResponse>>(mapped);
        }

        public async Task<ApiResponse<EmployeeResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            // Check
            var employee = await unitOfWork.EmployeeRepository.GetById(cancellationToken, request.EmployeeId);
            if(employee == null)
                return new ApiResponse<EmployeeResponse>("Employee not found.");
            
            // Response
            EmployeeResponse mapped = mapper.Map<EmployeeResponse>(employee);
            return new ApiResponse<EmployeeResponse>(mapped);
        }
    }
}