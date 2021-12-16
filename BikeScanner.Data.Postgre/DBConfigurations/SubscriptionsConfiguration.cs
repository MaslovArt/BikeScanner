using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.Data.Postgre.DBConfigurations
{
    internal class SubsciptionsConfiguration : IEntityTypeConfiguration<SubscriptionEntity>
    {
        public void Configure(EntityTypeBuilder<SubscriptionEntity> builder)
        {
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.Status);

            builder.Property(e => e.SearchQuery).IsRequired();
            builder.Property(e => e.NotificationType).IsRequired();
        }
    }
}
