using System;

namespace Izumi.Data.Enums
{
    public enum BannerRarityType : byte
    {
        Common = 1,
        Rare = 2,
        Animated = 3,
        Event = 4,
        Personal = 5
    }

    public static class BannerRarityHelper
    {
        public static string Localize(this BannerRarityType rarity) => rarity switch
        {
            BannerRarityType.Common => "Обычный",
            BannerRarityType.Rare => "Редкий",
            BannerRarityType.Animated => "Анимированный",
            BannerRarityType.Event => "Баннер",
            BannerRarityType.Personal => "Персональный",
            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
        };
    }
}
