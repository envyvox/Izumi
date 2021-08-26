using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource.Ingredients
{
    public class DrinkIngredient : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }
        public IngredientCategoryType Category { get; set; }
        public Guid IngredientId { get; set; }
        public uint Amount { get; set; }

        public Guid DrinkId { get; set; }
        public Drink Drink { get; set; }
    }

    public class DrinkIngredientConfiguration : IEntityTypeConfiguration<DrinkIngredient>
    {
        public void Configure(EntityTypeBuilder<DrinkIngredient> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.DrinkId, x.Category, x.IngredientId });

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.IngredientId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();

            builder
                .HasOne(x => x.Drink)
                .WithMany()
                .HasForeignKey(x => x.DrinkId);
        }
    }
}
