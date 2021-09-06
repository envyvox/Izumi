﻿using Izumi.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities
{
    public class WorldProperty
    {
        public WorldPropertyType Type { get; set; }
        public uint Value { get; set; }
    }

    public class WorldPropertyConfiguration : IEntityTypeConfiguration<WorldProperty>
    {
        public void Configure(EntityTypeBuilder<WorldProperty> builder)
        {
            builder.HasKey(x => x.Type);
            builder.HasIndex(x => x.Type).IsUnique();

            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Value).IsRequired();
        }
    }
}
