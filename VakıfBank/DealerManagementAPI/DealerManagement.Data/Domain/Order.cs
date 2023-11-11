
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Order : BaseModel
    {
        public int CompanyId { get; set; }
        
        public virtual Company? Company { get; set; }

        public string SupplierIBAN { get; set; }

        public decimal UnitPriceSum { get; set; } 

        public decimal KDVPriceSum { get; set; }
        
        public decimal TotalPrice { get; set; }

        public string? BillingCode { get; set; }

        public string Status { get; set; }

        public virtual List<OrderItem>? OrderItems { get; set; }

        public virtual Payment? Payment { get; set; }


        public class OrderConfigruration : IEntityTypeConfiguration<Order>
        {
            public void Configure(EntityTypeBuilder<Order> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

                builder.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Cart");
                // Cart, Cancelled, Confirmed, Paid
                builder.Property(e => e.UnitPriceSum).IsRequired().HasDefaultValue(0).HasPrecision(18, 2);             
                builder.Property(e => e.KDVPriceSum).IsRequired().HasDefaultValue(0).HasPrecision(18, 2);         
                builder.Property(e => e.TotalPrice).IsRequired().HasDefaultValue(0).HasPrecision(18, 2);    
                builder.Property(e => e.SupplierIBAN).IsRequired(); 
                builder.Property(e => e.BillingCode).HasMaxLength(20);

                builder.HasOne(d => d.Company).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Order_Company");
            }
        }
    }
}