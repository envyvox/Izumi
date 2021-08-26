using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource.Properties
{
    public class CraftingProperty : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }
        public CraftingPropertyType Property { get; set; }
        public uint Value { get; set; }

        public Guid CraftingId { get; set; }
        public Crafting Crafting { get; set; }
    }

    public class CraftingPropertyConfiguration : IEntityTypeConfiguration<CraftingProperty>
    {
        public void Configure(EntityTypeBuilder<CraftingProperty> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.CraftingId, x.Property }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Property).IsRequired();
            builder.Property(x => x.Value).IsRequired();

            builder
                .HasOne(x => x.Crafting)
                .WithMany()
                .HasForeignKey(x => x.CraftingId);
        }
    }
}
