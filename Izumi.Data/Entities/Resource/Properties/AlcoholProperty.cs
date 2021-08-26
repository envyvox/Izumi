using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Resource.Properties
{
    public class AlcoholProperty : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }
        public AlcoholPropertyType Property { get; set; }
        public uint Value { get; set; }

        public Guid AlcoholId { get; set; }
        public Alcohol Alcohol { get; set; }
    }

    public class AlcoholPropertyConfiguration : IEntityTypeConfiguration<AlcoholProperty>
    {
        public void Configure(EntityTypeBuilder<AlcoholProperty> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.AlcoholId, x.Property }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Property).IsRequired();
            builder.Property(x => x.Value).IsRequired();

            builder
                .HasOne(x => x.Alcohol)
                .WithMany()
                .HasForeignKey(x => x.AlcoholId);
        }
    }
}
