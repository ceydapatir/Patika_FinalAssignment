
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Payment : BaseModel
    {
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int? CardId { get; set; }

        public virtual CompanyCard? Card { get; set; }

        public string PaymentType { get; set; }

        public string Status { get; set; }

        public DateTime? PaymentDate { get; set; }

        public class PaymentConfigruration : IEntityTypeConfiguration<Payment>
        {
            public void Configure(EntityTypeBuilder<Payment> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

                builder.Property(e => e.PaymentDate).HasColumnType("datetime").HasDefaultValue(null);
                builder.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Confirmation Awaited");
                builder.Property(e => e.PaymentType).IsRequired().HasMaxLength(20);

                // Confirmation Awaited / Paid
                builder.HasOne(d => d.Order).WithOne(p => p.Payment)
                    .HasForeignKey<Payment>(d => d.OrderId)
                    .HasConstraintName("FK_Payment_Order");
                    
                builder.HasOne(d => d.Card).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_CompanyCard");
            }
        }
    }
}