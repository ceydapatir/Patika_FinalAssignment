using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Address : BaseModel
    {
        public int CompanyId { get; set;}
        
        public virtual Company Company { get; set; }
        
        public string Country { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string PostalCode { get; set; }
        
        public class AddressConfigruration : IEntityTypeConfiguration<Address>
        {
            public void Configure(EntityTypeBuilder<Address> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

                builder.Property(e => e.AddressLine1).IsRequired().HasMaxLength(50);
                builder.Property(e => e.AddressLine2).HasMaxLength(50);
                builder.Property(e => e.City).IsRequired().HasMaxLength(20);
                builder.Property(e => e.Country).IsRequired().HasMaxLength(20);
                builder.Property(e => e.PostalCode).IsRequired().HasMaxLength(5);
                builder.Property(e => e.Province).IsRequired().HasMaxLength(20);
                builder.HasOne(d => d.Company).WithOne(p => p.CompanyAddress)
                    .HasForeignKey<Address>(d => d.CompanyId)
                    .HasConstraintName("FK_Address_Company");
            }
        }
    }
}