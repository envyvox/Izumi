using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities
{
    public class BuildingIngredient : IUniqueIdentifiedEntity, IAmountEntity
    {
        public Guid Id { get; set; }
        public IngredientCategoryType Category { get; set; }
        public Guid IngredientId { get; set; }
        public uint Amount { get; set; }

        public BuildingType BuildingType { get; set; }
        public Building Building { get; set; }
    }

    public class BuildingIngredientConfiguration : IEntityTypeConfiguration<BuildingIngredient>
    {
        public void Configure(EntityTypeBuilder<BuildingIngredient> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.BuildingType, x.Category, x.IngredientId }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.IngredientId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();

            builder
                .HasOne(x => x.Building)
                .WithMany(x => x.Ingredients)
                .HasForeignKey(x => x.BuildingType);
        }
    }
}
