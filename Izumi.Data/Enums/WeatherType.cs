using System;

namespace Izumi.Data.Enums
{
    public enum WeatherType : byte
    {
        Any = 0,
        Clear = 1,
        Rain = 2
    }

    public static class WeatherHelper
    {
        public static string Localize(this WeatherType weather) => weather switch
        {
            WeatherType.Any => "любой",
            WeatherType.Clear => "ясной",
            WeatherType.Rain => "дождливой",
            _ => throw new ArgumentOutOfRangeException(nameof(weather), weather, null)
        };
    }
}
