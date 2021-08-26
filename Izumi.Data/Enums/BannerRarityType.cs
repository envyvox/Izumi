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
            BannerRarityType.Common => "Обычный баннер",
            BannerRarityType.Rare => "Редкий баннер",
            BannerRarityType.Animated => "Анимированный баннер",
            BannerRarityType.Event => "Баннер события",
            BannerRarityType.Personal => "Персональный баннер",
            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
        };
    }
}
