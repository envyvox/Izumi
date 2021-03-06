using System;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource
{
    public class Product : IUniqueIdentifiedEntity, IAutoIncrementedEntity, INamedEntity, IPricedEntity
    {
        public Guid Id { get; set; }
        public long AutoIncrementedId { get; set; }
        public string Name { get; set; }
        public uint Price { get; set; }
    }

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.AutoIncrementedId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Price).IsRequired();
        }
    }
}
