using BikeScanner.Data.Postgre.DBConfigurations;
using BikeScanner.Domain.Content;
using BikeScanner.Domain.NotificationsQueue;
using BikeScanner.Domain.SearchHistory;
using BikeScanner.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.Data.Postgre
{
    public class BikeScannerContext : DbContext
    {
        public BikeScannerContext(DbContextOptions<BikeScannerContext> options)
            : base(options)
        { }

        public DbSet<ContentEntity> Contents { get; set; }
        public DbSet<NotificationQueueEntity> NotificationsQueue { get; set; }
        public DbSet<SearchHistoryEntity> SearchHistories { get; set; }
        public DbSet<SubscriptionEntity> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ContentsConfiguration());
            modelBuilder.ApplyConfiguration(new SubsciptionsConfiguration());
            modelBuilder.ApplyConfiguration(new SearchHistoryConfiguration());
        }
    }
}
