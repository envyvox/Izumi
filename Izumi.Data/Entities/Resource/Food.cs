using System;
using System.Collections.Generic;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource
{
    public class Food : IUniqueIdentifiedEntity, IAutoIncrementedEntity, INamedEntity
    {
        public Guid Id { get; set; }
        public long AutoIncrementedId { get; set; }
        public string Name { get; set; }
        public FoodCategoryType Category { get; set; }
        public bool RecipeSellable { get; set; }
        public bool IsSpecial { get; set; }
        public List<FoodIngredient> Ingredients { get; set; }
    }

    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.AutoIncrementedId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.RecipeSellable).IsRequired();
            builder.Property(x => x.IsSpecial).IsRequired();
        }
    }
}
