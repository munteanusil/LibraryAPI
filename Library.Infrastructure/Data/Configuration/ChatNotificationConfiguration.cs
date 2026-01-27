using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Data.Configuration
{

    public class ChatNotificationConfigurations : IEntityTypeConfiguration<ChatNotifications>
    {
        public void Configure(EntityTypeBuilder<ChatNotifications> builder)
        {
            builder.ToTable("chat_notifications");
            builder.HasKey(c => new { c.ChatId, c.NotificationId });

            builder.HasOne(c => c.Chat)
                .WithMany(c => c.ChatNotifications)
                .HasForeignKey(c => c.ChatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Notification)
                .WithMany(c => c.ChatsNotifications)
                .HasForeignKey(c => c.NotificationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
