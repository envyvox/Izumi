using System;
using System.Collections.Generic;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource
{
    public class Alcohol : IUniqueIdentifiedEntity, IAutoIncrementedEntity, INamedEntity
    {
        public Guid Id { get; set; }
        public long AutoIncrementedId { get; set; }
        public string Name { get; set; }
        public List<AlcoholProperty> Properties { get; set; }
        public List<AlcoholIngredient> Ingredients { get; set; }
    }

    public class AlcoholConfiguration : IEntityTypeConfiguration<Alcohol>
    {
        public void Configure(EntityTypeBuilder<Alcohol> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.AutoIncrementedId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
