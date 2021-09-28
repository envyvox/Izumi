using System.Collections.Generic;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities
{
    public class Building : INamedEntity
    {
        public BuildingCategoryType Category { get; set; }
        public BuildingType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<BuildingIngredient> Ingredients { get; set; }
    }

    public class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.HasKey(x => x.Type);
            builder.HasIndex(x => x.Type).IsUnique();

            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
        }
    }
}
