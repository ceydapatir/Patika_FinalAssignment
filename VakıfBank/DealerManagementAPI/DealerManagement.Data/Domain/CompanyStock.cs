
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class CompanyStock : BaseModel
    {
        public int CompanyId { get; set; }

        public virtual Company? Company { get; set; }

        public int ProductId { get; set; }

        public virtual Product? Product { get; set; }

        public int Stock { get; set; }

        public class CompanyStockConfigruration : IEntityTypeConfiguration<CompanyStock>
        {
            public void Configure(EntityTypeBuilder<CompanyStock> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
                             
                builder.Property(e => e.Stock).IsRequired();

                builder.HasOne(d => d.Company).WithMany(p => p.CompanyStocks)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_CompanyStock_Company");

                builder.HasOne(d => d.Product).WithMany(p => p.CompanyStocks)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_CompanyStock_Product");
            }
        }
    }
}