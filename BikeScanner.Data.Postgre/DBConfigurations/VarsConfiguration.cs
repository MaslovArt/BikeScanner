using BikeScanner.Domain.Vars;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.Data.Postgre.DBConfigurations
{
    internal class VarsConfiguration : IEntityTypeConfiguration<VarEntity>
    {
        public void Configure(EntityTypeBuilder<VarEntity> builder)
        {
            builder.HasKey(v => v.Key);
        }
    }
}
