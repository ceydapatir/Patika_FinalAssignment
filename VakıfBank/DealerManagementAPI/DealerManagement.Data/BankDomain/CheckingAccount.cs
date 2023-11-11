using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class CheckingAccount : BaseModel
    {
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }

        public decimal Balance { get; set; }

        public class CheckingAccountConfigruration : IEntityTypeConfiguration<CheckingAccount>
        {
            public void Configure(EntityTypeBuilder<CheckingAccount> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

                builder.Property(e => e.Balance).IsRequired().HasPrecision(18, 2);
                    
                builder.HasOne(d => d.Account).WithOne(p => p.CheckingAccount)
                    .HasForeignKey<CheckingAccount>(d => d.AccountId)
                    .HasConstraintName("FK_CheckingAccount_Account");
            }
        }
    }
}