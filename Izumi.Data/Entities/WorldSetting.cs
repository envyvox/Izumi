using Izumi.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities
{
    public class WorldSetting
    {
        public SeasonType CurrentSeason { get; set; }
        public WeatherType WeatherToday { get; set; }
        public WeatherType WeatherTomorrow { get; set; }
    }

    public class WorldSettingConfiguration : IEntityTypeConfiguration<WorldSetting>
    {
        public void Configure(EntityTypeBuilder<WorldSetting> builder)
        {
            builder.HasNoKey();

            builder.Property(x => x.CurrentSeason).IsRequired();
            builder.Property(x => x.WeatherToday).IsRequired();
            builder.Property(x => x.WeatherTomorrow).IsRequired();
        }
    }
}
