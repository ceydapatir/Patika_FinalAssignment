
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Employee : BaseModel
    {
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mail { get; set; }

        public string EmployeeNumber { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
        
        public virtual List<Message> Messages { get; set; } 

        public class EmployeeConfigruration : IEntityTypeConfiguration<Employee>
        {
            public void Configure(EntityTypeBuilder<Employee> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

                builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
                builder.Property(e => e.Mail).IsRequired().HasMaxLength(50);
                builder.Property(e => e.Password).IsRequired().HasMaxLength(50);
                builder.Property(e => e.Role).IsRequired().HasMaxLength(10).HasDefaultValue("dealer");
                builder.Property(e => e.Surname).IsRequired().HasMaxLength(50);
                
                builder.Property(e => e.EmployeeNumber).IsRequired().HasMaxLength(10);

                builder.HasOne(d => d.Company).WithMany(p => p.Employees).HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Employee_Company");
            }
        }

    }
}