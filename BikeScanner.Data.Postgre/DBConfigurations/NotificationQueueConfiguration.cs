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
                .Property(e => e.SearchQuery)
                .IsRequired();
            builder
                .Property(e => e.AdUrl)
                .IsRequired();
        }
    }
}
