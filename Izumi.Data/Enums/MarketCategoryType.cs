using System;

namespace Izumi.Data.Enums
{
    public enum MarketCategoryType : byte
    {
        Gathering = 1,
        Crafting = 2,
        Alcohol = 3,
        Drink = 4,
        Food = 5,
        Crop = 6
    }

    public static class MarketCategoryHelper
    {
        public static string Localize(this MarketCategoryType category) => category switch
        {
            MarketCategoryType.Gathering => InventoryCategoryType.Gathering.Localize(),
            MarketCategoryType.Crafting => InventoryCategoryType.Crafting.Localize(),
            MarketCategoryType.Alcohol => InventoryCategoryType.Alcohol.Localize(),
            MarketCategoryType.Drink => InventoryCategoryType.Drink.Localize(),
            MarketCategoryType.Food => InventoryCategoryType.Food.Localize(),
            MarketCategoryType.Crop => InventoryCategoryType.Crop.Localize(),
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
