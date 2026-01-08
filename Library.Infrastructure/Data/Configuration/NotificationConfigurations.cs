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
    public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("notifications");
            builder.HasKey(c => c.Id);
            builder.Property(c =>c.NotificationType)
                .HasConversion(new EnumToNumberConverter<NotificationType,int>());
            builder.HasMany(c =>c.ChatNotifications)
                .WithOne(c =>c.Notification)
                .HasForeignKey(c =>c.NotificationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
