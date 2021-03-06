using System;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.Data.Postgre.DBConfigurations
{
    internal class NotificationQueueConfiguration : IEntityTypeConfiguration<NotificationQueueEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationQueueEntity> builder)
        {
            builder.HasIndex(e => e.Status);

            builder
                .Property(e => e.Text)
                .IsRequired();
            builder
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<NotificationStatus>(v));
        }
    }
}

