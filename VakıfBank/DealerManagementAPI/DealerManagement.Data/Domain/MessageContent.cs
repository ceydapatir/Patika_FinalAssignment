
using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class MessageContent : BaseModel
    {
        public int MessageId { get; set; }
        public virtual Message Message { get; set; }
        public string Content { get; set; }
        public DateTime MessageDate { get; set; }

        public class MessageContentConfigruration : IEntityTypeConfiguration<MessageContent>
        {
            public void Configure(EntityTypeBuilder<MessageContent> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
                
                builder.Property(e => e.MessageDate).IsRequired().HasColumnType("datetime").HasDefaultValue(DateTime.UtcNow);
                builder.Property(e => e.Content).IsRequired().HasMaxLength(500);

                builder.HasOne(d => d.Message).WithMany(p => p.MessageContents)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK_MessageContent_Message");
            }
        }
    }
}