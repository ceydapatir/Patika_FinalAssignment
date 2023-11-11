
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Dealer : BaseModel
    {
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public decimal ProfitMargin { get; set; }
        public DateTime ContractDeadline { get; set; }

        public class DealerConfigruration : IEntityTypeConfiguration<Dealer>
        {
            public void Configure(EntityTypeBuilder<Dealer> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
                             
                builder.Property(e => e.ProfitMargin).IsRequired().HasDefaultValue(0).HasPrecision(18, 2);
                builder.Property(e => e.ContractDeadline).IsRequired();

                builder.HasOne(d => d.Company).WithOne(p => p.Dealer)
                    .HasForeignKey<Dealer>(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dealer_Company");

                builder.HasOne(d => d.Supplier).WithMany(p => p.Dealers)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dealer_Supplier");
            }
        }
    }
}