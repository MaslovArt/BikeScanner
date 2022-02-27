using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BikeScanner.Data.Postgre
{
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BikeScannerContext>
    {
        public BikeScannerContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.DesignTimeDb.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<BikeScannerContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new BikeScannerContext(optionsBuilder.Options);
        }
    }
}
