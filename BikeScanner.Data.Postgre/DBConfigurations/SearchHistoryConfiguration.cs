﻿using BikeScanner.Domain.SearchHistory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.Data.Postgre.DBConfigurations
{
    internal class SearchHistoryConfiguration : IEntityTypeConfiguration<SearchHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<SearchHistoryEntity> builder)
        {
            builder.HasIndex(e => e.UserId);
        }
    }
}
