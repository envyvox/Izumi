using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource.Ingredients
{
    public class AlcoholIngredient : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }
        public IngredientCategoryType Category { get; set; }
        public Guid IngredientId { get; set; }
        public uint Amount { get; set; }

        public Guid AlcoholId { get; set; }
        public Alcohol Alcohol { get; set; }
    }

    public class AlcoholIngredientConfiguration : IEntityTypeConfiguration<AlcoholIngredient>
    {
        public void Configure(EntityTypeBuilder<AlcoholIngredient> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.AlcoholId, x.Category, x.IngredientId });

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.IngredientId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();

            builder
                .HasOne(x => x.Alcohol)
                .WithMany()
                .HasForeignKey(x => x.AlcoholId);
        }
    }
}
