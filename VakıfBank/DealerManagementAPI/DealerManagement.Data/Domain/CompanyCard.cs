
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class CompanyCard : BaseModel
    {
        public string CardType { get; set; }
        
        public string CardName { get; set; }

        public string CardNumber { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
        
        public virtual List<Payment>? Payments { get; set; }

        public class CompanyCardConfigruration : IEntityTypeConfiguration<CompanyCard>
        {
            public void Configure(EntityTypeBuilder<CompanyCard> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
                
                builder.Property(e => e.CardName).IsRequired().HasMaxLength(20);
                builder.Property(e => e.CardNumber).IsRequired().HasMaxLength(16);

                builder.HasOne(d => d.Company).WithMany(p => p.Cards)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_CompanyCard_Company");
            }
        }
    }
}