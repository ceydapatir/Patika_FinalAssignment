using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Supplier : BaseModel
    {
        public int CompanyId {get; set; }
        
        public virtual Company Company { get; set; }

        public string IBAN { get; set; }

        public virtual List<Dealer>? Dealers { get; set; }
        
        public class SupplierConfigruration : IEntityTypeConfiguration<Supplier>
        {
            public void Configure(EntityTypeBuilder<Supplier> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
                builder.Property(e => e.IBAN).IsRequired().HasMaxLength(26);

                builder.HasOne(d => d.Company).WithOne(p => p.Supplier)
                    .HasForeignKey<Supplier>(d => d.CompanyId)
                    .HasConstraintName("FK_Supplier_Company");
            }
        }
    }
}