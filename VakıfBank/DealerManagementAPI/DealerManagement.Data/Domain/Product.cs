
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Product : BaseModel
    {        
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }
        
        public decimal UnitPrice { get; set; }

        public decimal KDV { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
        
        public virtual List<CompanyStock> CompanyStocks { get; set; }

        public class ProductConfigruration : IEntityTypeConfiguration<Product>
        {
            public void Configure(EntityTypeBuilder<Product> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

                builder.Property(e => e.ProductCode).IsRequired().HasMaxLength(20);
                builder.Property(e => e.ProductName).IsRequired().HasMaxLength(20);           
                builder.Property(e => e.KDV).IsRequired().HasDefaultValue(22);        
                builder.Property(e => e.Description).IsRequired().HasDefaultValue(500);
                builder.Property(e => e.UnitPrice).IsRequired().HasPrecision(18, 2);
            }
        }
    }
}