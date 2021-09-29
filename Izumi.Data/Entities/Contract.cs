using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities
{
    public class Contract : IUniqueIdentifiedEntity, IAutoIncrementedEntity, INamedEntity
    {
        public Guid Id { get; set; }
        public long AutoIncrementedId { get; set; }
        public LocationType Location { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public uint CurrencyReward { get; set; }
        public uint ReputationReward { get; set; }
        public uint EnergyCost { get; set; }
    }

    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Location);

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.AutoIncrementedId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Location).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Duration).IsRequired();
            builder.Property(x => x.CurrencyReward).IsRequired();
            builder.Property(x => x.ReputationReward).IsRequired();
            builder.Property(x => x.EnergyCost).IsRequired();
        }
    }
}
