using DealerManagement.Data.Domain;
using DealerManagement.Data.Repository;

namespace DealerManagement.Data.UoW
{
    public interface IUnitOfWork
{
    void Complete();
    void CompleteTransaction();
    
    IGenericRepository<Company> CompanyRepository { get; }
    IGenericRepository<CompanyCard> CompanyCardRepository { get; } 
    IGenericRepository<Dealer> DealerRepository { get; }
    IGenericRepository<Supplier> SupplierRepository { get; }
    IGenericRepository<Address> AddressRepository { get; }
    IGenericRepository<Product> ProductRepository { get; }
    IGenericRepository<CompanyStock> CompanyStockRepository { get; }
    IGenericRepository<Order> OrderRepository { get; }
    IGenericRepository<OrderItem> OrderItemRepository { get; }

    IGenericRepository<Employee> EmployeeRepository { get; }
    IGenericRepository<Message> MessageRepository { get; } 
    IGenericRepository<MessageContent> MessageContentRepository { get; } 
    
    IGenericRepository<Account> AccountRepository { get; } 
    IGenericRepository<Card> CardRepository { get; } 
    IGenericRepository<CheckingAccount> CheckingAccountRepository { get; } 
    IGenericRepository<DepositAccount> DepositAccountRepository { get; } 
    IGenericRepository<Payment> PaymentRepository { get; } 
}
}