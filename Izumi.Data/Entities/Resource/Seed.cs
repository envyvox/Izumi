using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource
{
    public class Seed : IUniqueIdentifiedEntity, IAutoIncrementedEntity, INamedEntity, IPricedEntity
    {
        public Guid Id { get; set; }
        public long AutoIncrementedId { get; set; }
        public string Name { get; set; }
        public SeasonType Season { get; set; }
        public uint GrowthDays { get; set; }
        public uint ReGrowthDays { get; set; }
        public bool IsMultiply { get; set; }
        public uint Price { get; set; }
    }

    public class SeedConfiguration : IEntityTypeConfiguration<Seed>
    {
        public void Configure(EntityTypeBuilder<Seed> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.AutoIncrementedId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Season).IsRequired();
            builder.Property(x => x.GrowthDays).IsRequired();
            builder.Property(x => x.ReGrowthDays).IsRequired();
            builder.Property(x => x.IsMultiply).IsRequired();
            builder.Property(x => x.Price).IsRequired();
        }
    }
}
