using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Data.Repository;
using Serilog;

namespace DealerManagement.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DealerManagementDBContext dbContext;

        public UnitOfWork(DealerManagementDBContext dbContext)
        {
            this.dbContext = dbContext;

            CompanyRepository = new GenericRepository<Company>(dbContext);
            CompanyCardRepository = new GenericRepository<CompanyCard>(dbContext);
            EmployeeRepository = new GenericRepository<Employee>(dbContext);
            DealerRepository = new GenericRepository<Dealer>(dbContext);
            SupplierRepository = new GenericRepository<Supplier>(dbContext);
            AddressRepository = new GenericRepository<Address>(dbContext);
            ProductRepository = new GenericRepository<Product>(dbContext);
            CompanyStockRepository = new GenericRepository<CompanyStock>(dbContext);
            OrderRepository = new GenericRepository<Order>(dbContext);
            OrderItemRepository = new GenericRepository<OrderItem>(dbContext);
            PaymentRepository = new GenericRepository<Payment>(dbContext);
            MessageRepository = new GenericRepository<Message>(dbContext);
            MessageContentRepository = new GenericRepository<MessageContent>(dbContext);
            
            AccountRepository = new GenericRepository<Account>(dbContext);
            CardRepository = new GenericRepository<Card>(dbContext);
            DepositAccountRepository = new GenericRepository<DepositAccount>(dbContext);
            CheckingAccountRepository = new GenericRepository<CheckingAccount>(dbContext);
        }

        public IGenericRepository<Company> CompanyRepository { get; private set; }
        public IGenericRepository<CompanyCard> CompanyCardRepository { get; private set; }
        public IGenericRepository<Dealer> DealerRepository { get; private set; }
        public IGenericRepository<Supplier> SupplierRepository { get; private set; }
        public IGenericRepository<Address> AddressRepository { get; private set; }
        public IGenericRepository<Product> ProductRepository { get; private set; }
        public IGenericRepository<CompanyStock> CompanyStockRepository { get; private set; }
        public IGenericRepository<Order> OrderRepository { get; private set; }
        public IGenericRepository<OrderItem> OrderItemRepository { get; private set; }

        public IGenericRepository<Employee> EmployeeRepository { get; private set; }
        public IGenericRepository<Message> MessageRepository { get; private set; }
        public IGenericRepository<MessageContent> MessageContentRepository { get; private set; }

        public IGenericRepository<Account> AccountRepository { get; private set; }
        public IGenericRepository<Card> CardRepository { get; private set; }
        public IGenericRepository<DepositAccount> DepositAccountRepository { get; private set; }
        public IGenericRepository<CheckingAccount> CheckingAccountRepository { get; private set; }
        public IGenericRepository<Payment> PaymentRepository { get; private set; }

            
        public void Complete()
        {
            dbContext.SaveChanges();
        }

        public void CompleteTransaction()
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Error("CompleteTransactionError", ex);
                }
            }
        }
    }
}