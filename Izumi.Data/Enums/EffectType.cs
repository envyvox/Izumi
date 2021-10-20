using System;

namespace Izumi.Data.Enums
{
    public enum EffectType : byte
    {
        Lottery = 1
    }

    public static class EffectHelper
    {
        public static string Localize(this EffectType effect) => effect switch
        {
            EffectType.Lottery => "Лотерейный билет",
            _ => throw new ArgumentOutOfRangeException(nameof(effect), effect, null)
        };
    }
}
