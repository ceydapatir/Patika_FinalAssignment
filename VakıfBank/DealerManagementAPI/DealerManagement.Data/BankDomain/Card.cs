
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Card : BaseModel
    {
        public string CardHolder { get; set; }

        public string CardNumber { get; set; }

        public string CVV { get; set; }

        public string ExpireDate { get; set; }

        public decimal? ExpenseLimit { get; set; }

        public int AccountId { get; set; }

        public virtual Account Account { get; set; }

        public class CardConfigruration : IEntityTypeConfiguration<Card>
        {
            public void Configure(EntityTypeBuilder<Card> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
                
                builder.Property(e => e.CardHolder).IsRequired().HasMaxLength(20);
                builder.Property(e => e.CVV).IsRequired().HasMaxLength(3);
                builder.Property(e => e.ExpireDate).IsRequired().HasMaxLength(5);
                builder.Property(e => e.CardNumber).IsRequired().HasMaxLength(16);
                builder.Property(e => e.ExpenseLimit).HasPrecision(18, 2);

                builder.HasOne(d => d.Account).WithOne(p => p.Card)
                    .HasForeignKey<Card>(d => d.AccountId)
                    .HasConstraintName("FK_Card_Account");
            }
        }
    }
}