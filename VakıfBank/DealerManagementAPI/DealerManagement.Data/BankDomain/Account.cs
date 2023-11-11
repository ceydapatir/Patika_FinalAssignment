using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Account : BaseModel
    {           
        public string AccountNumber { get; set; }

        public string IBAN { get; set; }

        public string CurrencyCode { get; set; }

        public virtual List<Payment> Payments { get; set; }  
        
        public virtual CheckingAccount CheckingAccount { get; set; }  

        public virtual DepositAccount DepositAccount { get; set; }

        public virtual Card Card { get; set; }


        public class AccountConfigruration : IEntityTypeConfiguration<Account>
        {
            public void Configure(EntityTypeBuilder<Account> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
                
                builder.Property(e => e.CurrencyCode).IsRequired().HasMaxLength(10).HasDefaultValue("TRY");
                builder.Property(e => e.AccountNumber).IsRequired().HasMaxLength(16);
                builder.Property(e => e.IBAN).IsRequired().HasMaxLength(26);
            }
        }
    }
}