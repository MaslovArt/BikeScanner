using BikeScanner.Data.Postgre.DBConfigurations;
using BikeScanner.Domain.Models;
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
        public DbSet<SubscriptionEntity> Subscriptions { get; set; }
        public DbSet<VarEntity> Vars { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ActionEntity> Actions { get; set; }
        public DbSet<DevMessageEntity> DevMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ContentsConfiguration());
            modelBuilder.ApplyConfiguration(new SubsciptionsConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationQueueConfiguration());
            modelBuilder.ApplyConfiguration(new VarsConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new ActionsConfiguration());
            modelBuilder.ApplyConfiguration(new DevMessagesConfiguration());
        }
    }
}
