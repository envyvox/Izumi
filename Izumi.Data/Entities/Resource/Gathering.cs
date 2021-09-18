using System;
using System.Collections.Generic;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource
{
    public class Gathering : IUniqueIdentifiedEntity, IAutoIncrementedEntity, INamedEntity, IPricedEntity
    {
        public Guid Id { get; set; }
        public long AutoIncrementedId { get; set; }
        public string Name { get; set; }
        public LocationType Location { get; set; }
        public uint Price { get; set; }
        public List<GatheringProperty> Properties { get; set; }
    }

    public class GatheringConfiguration : IEntityTypeConfiguration<Gathering>
    {
        public void Configure(EntityTypeBuilder<Gathering> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.AutoIncrementedId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Location).IsRequired();
            builder.Property(x => x.Price).IsRequired();
        }
    }
}
