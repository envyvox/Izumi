using System;

namespace Izumi.Data.Enums
{
    public enum FishRarityType : byte
    {
        Common = 1,
        Rare = 2,
        Epic = 3,
        Mythical = 4,
        Legendary = 5
    }

    public static class FishRarityHelper
    {
        public static string Localize(this FishRarityType rarity, bool declension = false) => rarity switch
        {
            FishRarityType.Common => declension ? "обычную" : "Обычная",
            FishRarityType.Rare => declension ? "редкую" : "Редкая",
            FishRarityType.Epic => declension ? "эпическую" : "Эпическая",
            FishRarityType.Mythical => declension ? "мифическую" : "Мифическая",
            FishRarityType.Legendary => declension ? "легендарную" : "Легендарная",
            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
        };
    }
}
