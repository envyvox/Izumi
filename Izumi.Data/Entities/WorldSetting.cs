using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities
{
    public class WorldSetting : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }
        public SeasonType CurrentSeason { get; set; }
        public WeatherType WeatherToday { get; set; }
        public WeatherType WeatherTomorrow { get; set; }
    }

    public class WorldSettingConfiguration : IEntityTypeConfiguration<WorldSetting>
    {
        public void Configure(EntityTypeBuilder<WorldSetting> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CurrentSeason).IsRequired();
            builder.Property(x => x.WeatherToday).IsRequired();
            builder.Property(x => x.WeatherTomorrow).IsRequired();
        }
    }
}
