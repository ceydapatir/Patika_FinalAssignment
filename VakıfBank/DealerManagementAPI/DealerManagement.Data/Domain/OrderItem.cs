
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class OrderItem : BaseModel
    {
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int Amount { get; set; }

        public class OrderItemConfigruration : IEntityTypeConfiguration<OrderItem>
        {
            public void Configure(EntityTypeBuilder<OrderItem> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

                builder.Property(e => e.Amount).IsRequired();

                builder.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OrderItem_Product");

                builder.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderItem_Order");
            }
        }
    }
}