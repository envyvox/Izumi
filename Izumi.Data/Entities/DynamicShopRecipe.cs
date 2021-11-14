using System;
using Izumi.Data.Entities.Resource;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities
{
    public class DynamicShopRecipe : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }

        public Guid FoodId { get; set; }
        public Food Food { get; set; }
    }

    public class DynamicShopRecipeConfiguration : IEntityTypeConfiguration<DynamicShopRecipe>
    {
        public void Configure(EntityTypeBuilder<DynamicShopRecipe> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.FoodId).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();

            builder
                .HasOne(x => x.Food)
                .WithOne()
                .HasForeignKey<DynamicShopRecipe>(x => x.FoodId);
        }
    }
}