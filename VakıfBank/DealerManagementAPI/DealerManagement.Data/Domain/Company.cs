using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Company : BaseModel
    {
        public string CompanyName { get; set; }
        
        public string Mail { get; set; }      
        
        public virtual List<CompanyCard>? Cards { get; set; }
        
        public virtual List<Employee>? Employees { get; set; }
        
        public virtual List<Order>? Orders { get; set; }
        
        public virtual List<CompanyStock>? CompanyStocks { get; set; }
        
        public virtual Address? CompanyAddress { get; set; }  

        public virtual Supplier? Supplier { get; set; }
        
        public virtual Dealer? Dealer { get; set; }

        public class CompanyConfigruration : IEntityTypeConfiguration<Company>
        {
            public void Configure(EntityTypeBuilder<Company> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

                builder.Property(e => e.CompanyName).IsRequired().HasMaxLength(50);
                builder.Property(e => e.Mail).IsRequired().HasMaxLength(50);
            }
        }
    }
}