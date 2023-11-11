using Microsoft.EntityFrameworkCore;
using DealerManagement.Data.Domain;
using DealerManagement.Data.Seed;
using Microsoft.EntityFrameworkCore.Design;

namespace DealerManagement.Data.Context
{
    public class DealerManagementDBContext : DbContext
    {
        public DealerManagementDBContext(DbContextOptions<DealerManagementDBContext> options) : base(options) {}
        
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CheckingAccount> CheckingAccounts { get; set; }
        public DbSet<DepositAccount> DepositAccounts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Address> Addresses { get; set;}
        public DbSet<CompanyCard> CompanyCards { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Supplier> Supliers { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CompanyStock> CompanyStocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set;}
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageContent> MessageContents { get; set; }

        public class DealerManagementDBContextFactory : IDesignTimeDbContextFactory<DealerManagementDBContext>
        {
            public DealerManagementDBContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<DealerManagementDBContext>();
                optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=DealerManagementDB;Trusted_Connection=true;TrustServerCertificate=True;MultipleActiveResultSets=true;");

                return new DealerManagementDBContext(optionsBuilder.Options);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=DealerManagementDB;Trusted_Connection=true;TrustServerCertificate=True;MultipleActiveResultSets=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepositAccount.DepositAccountConfigruration());
            modelBuilder.ApplyConfiguration(new CheckingAccount.CheckingAccountConfigruration());
            modelBuilder.ApplyConfiguration(new Account.AccountConfigruration());
            modelBuilder.ApplyConfiguration(new Card.CardConfigruration());

            modelBuilder.ApplyConfiguration(new Address.AddressConfigruration());
            modelBuilder.ApplyConfiguration(new CompanyCard.CompanyCardConfigruration());
            modelBuilder.ApplyConfiguration(new Supplier.SupplierConfigruration());
            modelBuilder.ApplyConfiguration(new Company.CompanyConfigruration());
            modelBuilder.ApplyConfiguration(new CompanyStock.CompanyStockConfigruration());
            modelBuilder.ApplyConfiguration(new Dealer.DealerConfigruration());
            modelBuilder.ApplyConfiguration(new Message.MessageConfigruration());
            modelBuilder.ApplyConfiguration(new MessageContent.MessageContentConfigruration());
            modelBuilder.ApplyConfiguration(new Order.OrderConfigruration());
            modelBuilder.ApplyConfiguration(new OrderItem.OrderItemConfigruration());
            modelBuilder.ApplyConfiguration(new Payment.PaymentConfigruration());
            modelBuilder.ApplyConfiguration(new Product.ProductConfigruration());
            modelBuilder.ApplyConfiguration(new Employee.EmployeeConfigruration());

            
            new SeedData(modelBuilder).Seed();
            base.OnModelCreating(modelBuilder);
        }
    }
}