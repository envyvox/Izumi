using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource.Properties
{
    public class GatheringProperty : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }
        public GatheringPropertyType Property { get; set; }
        public uint Value { get; set; }

        public Guid GatheringId { get; set; }
        public Gathering Gathering { get; set; }
    }

    public class GatheringPropertyConfiguration : IEntityTypeConfiguration<GatheringProperty>
    {
        public void Configure(EntityTypeBuilder<GatheringProperty> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.GatheringId, x.Property }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Property).IsRequired();
            builder.Property(x => x.Value).IsRequired();

            builder
                .HasOne(x => x.Gathering)
                .WithMany(x => x.Properties)
                .HasForeignKey(x => x.GatheringId);
        }
    }
}
