using System;

namespace Izumi.Data.Enums
{
    public enum SeasonType : byte
    {
        Any = 0,
        Spring = 1,
        Summer = 2,
        Autumn = 3,
        Winter = 4
    }

    public static class SeasonHelper
    {
        public static string Localize(this SeasonType season) => season switch
        {
            SeasonType.Any => "Любой",
            SeasonType.Spring => "Весна",
            SeasonType.Summer => "Лето",
            SeasonType.Autumn => "Осень",
            SeasonType.Winter => "Зима",
            _ => throw new ArgumentOutOfRangeException(nameof(season), season, null)
        };

        public static string EmoteName(this SeasonType season) => "Season" + season;
    }
}
