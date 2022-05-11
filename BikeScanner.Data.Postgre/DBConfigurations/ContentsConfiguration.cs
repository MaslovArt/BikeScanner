using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.Data.Postgre.DBConfigurations
{
    internal class ContentsConfiguration : IEntityTypeConfiguration<ContentEntity>
    {
        public void Configure(EntityTypeBuilder<ContentEntity> builder)
        {
            builder.HasAlternateKey(e => e.Url);

            builder.HasIndex(e => e.Created);
            builder.HasIndex(e => e.Published);

            builder.Property(e => e.Text).IsRequired();
            builder.Property(e => e.Url).IsRequired();
        }
    }
}
