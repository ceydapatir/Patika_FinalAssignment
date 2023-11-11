using DealerManagement.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealerManagement.Data.Domain
{
    public class Message : BaseModel
    {
        public int SenderId { get; set; }
        public virtual Employee Sender { get; set; }
        public int ReceiverId { get; set; }
        public List<MessageContent>? MessageContents { get; set; }


        public class MessageConfigruration : IEntityTypeConfiguration<Message>
        {
            public void Configure(EntityTypeBuilder<Message> builder)
            {
                builder.Property(x => x.InsertDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

                builder.HasOne(d => d.Sender).WithMany(p => p.Messages)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("FK_Message_Sender");
            }
        }
    }
}