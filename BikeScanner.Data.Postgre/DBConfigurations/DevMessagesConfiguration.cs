using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.Data.Postgre.DBConfigurations
{
    internal class DevMessagesConfiguration : IEntityTypeConfiguration<DevMessageEntity>
    {
        public void Configure(EntityTypeBuilder<DevMessageEntity> builder)
        {
            
        }
    }
}

