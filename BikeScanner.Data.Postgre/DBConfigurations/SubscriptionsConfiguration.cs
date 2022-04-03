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
            builder
                .HasIndex(e => new { e.UserId, e.SearchQuery })
                .IsUnique();

            builder
                .Property(e => e.SearchQuery)
                .IsRequired();
        }
    }
}
