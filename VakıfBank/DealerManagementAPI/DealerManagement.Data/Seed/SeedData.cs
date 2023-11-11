using DealerManagement.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace DealerManagement.Data.Seed
{
    public class SeedData
    {
        private readonly ModelBuilder _modelBuilder;

        public SeedData(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            _modelBuilder.Entity<Address>().HasData(
                new Address { Id = 1, CompanyId = 1, Country = "Turkey", Province = "Ankara", City = "Yenimahalle", AddressLine1 = "İvedik Sanayi", AddressLine2 = null, PostalCode = "06370" },
                new Address { Id = 2, CompanyId = 2, Country = "Turkey", Province = "Ankara", City = "Yenimahalle", AddressLine1 = "Ostim", AddressLine2 = null, PostalCode = "06370" }
            );
            _modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Mail = "salihekin@gmail.com",  CompanyName = "EGE AKÜ"},
                new Company { Id = 2, Mail = "mervetopuz@gmail.com", CompanyName = "EGE AKÜ BAYİ 1"}
            );
            _modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, CompanyId = 1, IBAN = "TR123456789012345678901234"}
            );
            _modelBuilder.Entity<Dealer>().HasData(
                new Dealer { Id = 1, CompanyId = 2, SupplierId = 1, ProfitMargin = (decimal)10.2, ContractDeadline = DateTime.Now.AddYears(2) }
            );
            _modelBuilder.Entity<CompanyCard>().HasData(
                new CompanyCard { Id = 1, CompanyId = 2, CardName = "Card 1", CardNumber = "1234567890123456", CardType = "credit"},
                new CompanyCard { Id = 2, CompanyId = 2, CardName = "Card 2", CardNumber = "1234567890123457", CardType = "deposit"}
            );
            _modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, ProductCode = "10001", ProductName = "Ürün 1", Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusamus veniam corporis magnam aliquid officia explicabo expedita. Veritatis qui ipsum odio nihil facilis velit suscipit? A iusto incidunt nostrum nam alias.", UnitPrice = (decimal)10.00, KDV = 22 },
                new Product { Id = 2, ProductCode = "10002", ProductName = "Ürün 2", Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusamus veniam corporis magnam aliquid officia explicabo expedita. Veritatis qui ipsum odio nihil facilis velit suscipit? A iusto incidunt nostrum nam alias.", UnitPrice = (decimal)20.00, KDV = 22 }
            );
            _modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, CompanyId = 2, SupplierIBAN = "TR123456789012345678901234" }
            );
            _modelBuilder.Entity<CompanyStock>().HasData(
                new CompanyStock { Id = 1, CompanyId = 1, ProductId = 1, Stock = 50 },
                new CompanyStock { Id = 2, CompanyId = 1, ProductId = 2, Stock = 30 },        
                new CompanyStock { Id = 3, CompanyId = 2, ProductId = 1, Stock = 10 }
            );

            _modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Salih", Surname = "Ekin",  CompanyId = 1, Mail = "salihekin@gmail.com", EmployeeNumber = "10001", Password = "5d41402abc4b2a76b9719d911017c592", Role = "admin" },
                new Employee { Id = 2, Name = "Merve", Surname = "Topuz", CompanyId = 2, Mail = "mervetopuz@gmail.com", EmployeeNumber = "10002", Password = "5d41402abc4b2a76b9719d911017c592", Role = "dealer" }
            );

            _modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, AccountNumber = "9012345678901234", IBAN = "TR123456789012345678901234", CurrencyCode = "TRY"},
                new Account { Id = 2, AccountNumber = "1012345678901234", IBAN = "TR123456781012345678901234", CurrencyCode = "TRY"},
                new Account { Id = 3, AccountNumber = "2012345678901234", IBAN = "TR123456782012345678901234", CurrencyCode = "TRY"}
            );
            _modelBuilder.Entity<CheckingAccount>().HasData(
                new CheckingAccount { Id = 1, AccountId = 1, Balance = 0},
                new CheckingAccount { Id = 2, AccountId = 2, Balance = 1000}
            );
            _modelBuilder.Entity<DepositAccount>().HasData(
                new DepositAccount { Id = 1, AccountId = 3, OpeningAmount = 1000, Interest = 12, DebtTotal = 0 }
            );
            _modelBuilder.Entity<Card>().HasData(
                new Card { Id = 1, AccountId = 2, CardHolder = "Merve Topuz",  ExpenseLimit = 10000, CardNumber = "1234567890123456", CVV = "123", ExpireDate = "12/28"},
                new Card { Id = 2, AccountId = 3, CardHolder = "Yiğit Aslan",  ExpenseLimit = 10000, CardNumber = "1234567890123457", CVV = "321", ExpireDate = "12/28"}
            );
            
        }
    }
}