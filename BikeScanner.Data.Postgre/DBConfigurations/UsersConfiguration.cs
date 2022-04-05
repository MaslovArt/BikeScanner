using System;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.Data.Postgre.DBConfigurations
{
    internal class UsersConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasAlternateKey(e => e.UserId);
            builder.HasIndex(e => e.State);
            builder
                .Property(e => e.State)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<AccountState>(v));
        }
    }
}
