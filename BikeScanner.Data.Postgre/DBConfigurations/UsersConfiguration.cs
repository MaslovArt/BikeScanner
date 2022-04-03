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
            builder.HasIndex(e => e.AccountStatus);
            builder
                .Property(e => e.AccountStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<AccountStatus>(v));
        }
    }
}
