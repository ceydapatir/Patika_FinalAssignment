
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class DepositAccount : BaseModel
    {
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        
        public decimal OpeningAmount { get; set; }
        
        public decimal DebtTotal { get; set; }

        public decimal Interest { get; set; }        

        public class DepositAccountConfigruration : IEntityTypeConfiguration<DepositAccount>
        {
            public void Configure(EntityTypeBuilder<DepositAccount> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

                builder.Property(e => e.OpeningAmount).IsRequired().HasPrecision(18, 2);
                builder.Property(e => e.DebtTotal).IsRequired().HasDefaultValue(0).HasPrecision(18, 2);
                builder.Property(e => e.Interest).IsRequired().HasPrecision(18, 2);
                
                builder.HasOne(d => d.Account).WithOne(p => p.DepositAccount)
                    .HasForeignKey<DepositAccount>(d => d.AccountId)
                    .HasConstraintName("FK_DepositAccount_Account");
            }
        }
    }
}