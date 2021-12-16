using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.Data.Postgre.DBConfigurations
{
    internal class SearchHistoryConfiguration : IEntityTypeConfiguration<SearchHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<SearchHistoryEntity> builder)
        {
            builder.HasIndex(e => e.UserId);

            builder.Property(e => e.SearchQuery).IsRequired();
        }
    }
}
