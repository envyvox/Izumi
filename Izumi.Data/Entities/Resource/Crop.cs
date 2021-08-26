using System;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource
{
    public class Crop : IUniqueIdentifiedEntity, IAutoIncrementedEntity, INamedEntity, IPricedEntity
    {
        public Guid Id { get; set; }
        public long AutoIncrementedId { get; set; }
        public string Name { get; set; }
        public uint Price { get; set; }

        public Guid SeedId { get; set; }
        public Seed Seed { get; set; }
    }

    public class CropConfiguration : IEntityTypeConfiguration<Crop>
    {
        public void Configure(EntityTypeBuilder<Crop> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.AutoIncrementedId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Price).IsRequired();

            builder
                .HasOne(x => x.Seed)
                .WithOne()
                .HasForeignKey<Crop>(x => x.SeedId);
        }
    }
}
