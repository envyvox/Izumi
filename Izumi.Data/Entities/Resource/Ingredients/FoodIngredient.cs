using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource.Ingredients
{
    public class FoodIngredient : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }
        public IngredientCategoryType Category { get; set; }
        public Guid IngredientId { get; set; }
        public uint Amount { get; set; }

        public Guid FoodId { get; set; }
        public Food Food { get; set; }
    }

    public class FoodIngredientConfiguration : IEntityTypeConfiguration<FoodIngredient>
    {
        public void Configure(EntityTypeBuilder<FoodIngredient> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.FoodId, x.Category, x.IngredientId });

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.IngredientId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();

            builder
                .HasOne(x => x.Food)
                .WithMany()
                .HasForeignKey(x => x.FoodId);
        }
    }
}
