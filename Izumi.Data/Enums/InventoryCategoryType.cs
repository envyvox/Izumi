using System;

namespace Izumi.Data.Enums
{
    public enum InventoryCategoryType : byte
    {
        Currency = 1,
        Box = 2,
        Gathering = 3,
        Product = 4,
        Crafting = 5,
        Alcohol = 6,
        Drink = 7,
        Seed = 8,
        Crop = 9,
        Fish = 10,
        Food = 11,
        Seafood = 12
    }

    public static class InventoryCategoryHelper
    {
        public static string Localize(this InventoryCategoryType category) => category switch
        {
            InventoryCategoryType.Currency => "Валюта",
            InventoryCategoryType.Gathering => "Собирательские ресурсы",
            InventoryCategoryType.Product => "Продукты",
            InventoryCategoryType.Crafting => "Изготавливаемые предметы",
            InventoryCategoryType.Alcohol => "Алкоголь",
            InventoryCategoryType.Drink => "Напитки",
            InventoryCategoryType.Seed => "Семена",
            InventoryCategoryType.Crop => "Урожай",
            InventoryCategoryType.Fish => "Рыба",
            InventoryCategoryType.Food => "Блюда",
            InventoryCategoryType.Box => "Коробки",
            InventoryCategoryType.Seafood => "Морепродукты",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
