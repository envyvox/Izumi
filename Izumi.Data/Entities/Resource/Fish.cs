using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Izumi.Data.Entities.Resource
{
    public class Fish : IUniqueIdentifiedEntity, IAutoIncrementedEntity, INamedEntity, IPricedEntity
    {
        public Guid Id { get; set; }
        public long AutoIncrementedId { get; set; }
        public string Name { get; set; }
        public FishRarityType Rarity { get; set; }
        public WeatherType CatchWeather { get; set; }
        public TimesDayType CatchTimesDay { get; set; }
        public List<SeasonType> CatchSeasons { get; set; }
        public uint Price { get; set; }
    }

    public class FishConfiguration : IEntityTypeConfiguration<Fish>
    {
        public void Configure(EntityTypeBuilder<Fish> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.AutoIncrementedId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Rarity).IsRequired();
            builder.Property(x => x.CatchWeather).IsRequired();
            builder.Property(x => x.CatchTimesDay).IsRequired();

            builder
                .Property(x => x.CatchSeasons)
                .IsRequired()
                .HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<SeasonType>>(v, null));

            builder.Property(x => x.Price).IsRequired();
        }
    }
}
