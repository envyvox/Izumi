using System;
using System.Collections.Generic;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource
{
    public class Crafting : IUniqueIdentifiedEntity, IAutoIncrementedEntity, INamedEntity
    {
        public Guid Id { get; set; }
        public long AutoIncrementedId { get; set; }
        public string Name { get; set; }
        public List<CraftingProperty> Properties { get; set; }
        public List<CraftingIngredient> Ingredients { get; set; }
    }

    public class CraftingConfiguration : IEntityTypeConfiguration<Crafting>
    {
        public void Configure(EntityTypeBuilder<Crafting> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.AutoIncrementedId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
