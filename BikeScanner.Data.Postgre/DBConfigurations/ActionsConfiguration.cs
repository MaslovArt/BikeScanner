using System;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.Data.Postgre.DBConfigurations
{
    internal class ActionsConfiguration : IEntityTypeConfiguration<ActionEntity>
    {
        public void Configure(EntityTypeBuilder<ActionEntity> builder)
        {
            builder
                .Property(e => e.Action)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<ActionType>(v));
        }
    }
}

