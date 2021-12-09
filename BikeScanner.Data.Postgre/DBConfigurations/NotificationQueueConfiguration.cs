using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.Data.Postgre.DBConfigurations
{
    internal class NotificationQueueConfiguration : IEntityTypeConfiguration<NotificationQueueEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationQueueEntity> builder)
        {

        }
    }
}
