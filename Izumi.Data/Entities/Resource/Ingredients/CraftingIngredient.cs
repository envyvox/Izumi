using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource.Ingredients
{
    public class CraftingIngredient : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }
        public IngredientCategoryType Category { get; set; }
        public Guid IngredientId { get; set; }
        public uint Amount { get; set; }

        public Guid CraftingId { get; set; }
        public Crafting Crafting { get; set; }
    }

    public class CraftingIngredientConfiguration : IEntityTypeConfiguration<CraftingIngredient>
    {
        public void Configure(EntityTypeBuilder<CraftingIngredient> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.CraftingId, x.Category, x.IngredientId });

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.IngredientId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();

            builder
                .HasOne(x => x.Crafting)
                .WithMany(x => x.Ingredients)
                .HasForeignKey(x => x.CraftingId);
        }
    }
}
