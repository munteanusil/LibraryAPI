using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Data.Configuration
{
    public class ChatConfigurations : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("chats");
            builder.HasKey(b => b.Id);

            builder.Property(p => p.FirstName)
                .HasMaxLength(256);

            builder.Property(p => p.LastName)
                .HasMaxLength(256);

            builder.Property(p => p.UserName)
                .HasMaxLength(256)
                .HasDefaultValue(null);

            builder.Property(b => b.IsForm)
                .HasDefaultValue(false);
            builder.Property(b => b.Type)
                .HasMaxLength(100);

            builder.HasMany(p => p.ChatNotifications)
                .WithOne(b => b.Chat)
                .HasForeignKey(b => b.ChatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
