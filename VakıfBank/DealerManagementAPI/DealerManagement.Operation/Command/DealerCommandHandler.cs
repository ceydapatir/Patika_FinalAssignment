
using AutoMapper;
using DealerManagement.Base.Response;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.UoW;
using DealerManagement.Operation.Validation;
using DealerManagement.Schema.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DealerManagement.Operation.Cqrs.DealerCqrs;

namespace DealerManagement.Operation.Command
{
    public class DealerCommandHandler :
        IRequestHandler<CreateDealerCommand, ApiResponse<DealerResponse>>,
        IRequestHandler<UpdateDealerCommand, ApiResponse<DealerResponse>>,
        IRequestHandler<UpdateProfitMarginCommand, ApiResponse<DealerResponse>>,
        IRequestHandler<DeleteDealerCommand, ApiResponse>
    {
        private readonly DealerManagementDBContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public DealerCommandHandler(DealerManagementDBContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<DealerResponse>> Handle(CreateDealerCommand request, CancellationToken cancellationToken)
        {
            // Validation
            DealerValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<DealerResponse>(result.Errors[0].ToString());

            // Check
            Company company = await dbContext.Set<Company>().Include(x => x.Dealer).FirstOrDefaultAsync(x => x.CompanyName == request.Model.CompanyName, cancellationToken);
            if(company is not null)
                return new ApiResponse<DealerResponse>("Dealer already exist.");

            // Create
            Company mapped = mapper.Map<Company>(request.Model);
            var company_id = await unitOfWork.CompanyRepository.InsertAndGetId(cancellationToken, mapped);
            
            Company admin_company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Supplier"); // to add supplier
            Dealer dealer = new Dealer(){ CompanyId = company_id, SupplierId = admin_company.Supplier.Id, ProfitMargin = (decimal)request.Model.ProfitMargin, ContractDeadline = request.Model.ContractDeadline};
            unitOfWork.DealerRepository.Insert(dealer);

            Company supplier = await dbContext.Set<Company>().Include(x => x.Supplier).FirstOrDefaultAsync(x => x.Id == request.CompanyId, cancellationToken);
            Order order = new Order(){CompanyId = company_id, SupplierIBAN = supplier.Supplier.IBAN}; // for cart
            unitOfWork.OrderRepository.Insert(order);

            // Response
            var response = mapper.Map<DealerResponse>(mapped);
            return new ApiResponse<DealerResponse>(response);
        }

        public async Task<ApiResponse<DealerResponse>> Handle(UpdateDealerCommand request, CancellationToken cancellationToken)
        {
            // Validation
            DealerUpdateValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<DealerResponse>(result.Errors[0].ToString());

            // Check
            var company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Dealer");
            if(company == null)
                return new ApiResponse<DealerResponse>("Dealer not found.");
            
            // Update
            company.Mail = !request.Model.Mail.Equals("") ? request.Model.Mail : company.Mail;
            await dbContext.SaveChangesAsync(cancellationToken);

            // Response
            var response = mapper.Map<DealerResponse>(company);
            return new ApiResponse<DealerResponse>(response);
        }

        public async Task<ApiResponse<DealerResponse>> Handle(UpdateProfitMarginCommand request, CancellationToken cancellationToken)
        {
            // Validation
            UpdateProfitMarginValidator validator = new();
            var result = validator.Validate(request.Model);
            if(!result.IsValid)
                return new ApiResponse<DealerResponse>(result.Errors[0].ToString());

            // Check
            var company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Dealer");
            if(company == null)
                return new ApiResponse<DealerResponse>("Dealer not found.");
            
            // Update
            company.Dealer.ProfitMargin = !request.Model.ProfitMargin.Equals("") ? (decimal)request.Model.ProfitMargin : company.Dealer.ProfitMargin;
            await dbContext.SaveChangesAsync(cancellationToken);
            
            //Response
            var response = mapper.Map<DealerResponse>(company);
            return new ApiResponse<DealerResponse>(response);
        }

        public async Task<ApiResponse> Handle(DeleteDealerCommand request, CancellationToken cancellationToken)
        {
            // Check
            var company = await unitOfWork.CompanyRepository.GetById(cancellationToken, request.CompanyId, "Dealer", "CompanyAddress", "Orders", "Cards", "Employees", "CompanyStocks");
            if(company == null)
                return new ApiResponse("Dealer not found.");
            
            // Delete
            unitOfWork.OrderRepository.RemoveRange(company.Orders);
            unitOfWork.CompanyCardRepository.RemoveRange(company.Cards);
            unitOfWork.EmployeeRepository.RemoveRange(company.Employees);
            unitOfWork.CompanyStockRepository.RemoveRange(company.CompanyStocks);
            unitOfWork.AddressRepository.Remove(company.CompanyAddress.Id);
            unitOfWork.DealerRepository.Remove(company.Dealer.Id);
            unitOfWork.CompanyRepository.Remove(request.CompanyId);
            
            return new ApiResponse();
        }
    }
}