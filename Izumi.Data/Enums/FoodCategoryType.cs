using System;

namespace Izumi.Data.Enums
{
    public enum FoodCategoryType : byte
    {
        Newbie = 1,
        Student = 2,
        Experienced = 3,
        Professional = 4,
        Expert = 5,
        Master = 6,
        Grandmaster = 7,
        Legendary = 8
    }

    public static class FoodCategoryHelper
    {
        public static string Localize(this FoodCategoryType category) => "Блюда " + category switch
        {
            FoodCategoryType.Newbie => "начинающего повара",
            FoodCategoryType.Student => "повара-ученика",
            FoodCategoryType.Experienced => "опытного повара",
            FoodCategoryType.Professional => "повара-профессионала",
            FoodCategoryType.Expert => "повара-эксперта",
            FoodCategoryType.Master => "мастера-повара",
            FoodCategoryType.Grandmaster => "повара-грандмастера",
            FoodCategoryType.Legendary => "легендарного повара",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
