using System;

namespace Izumi.Data.Enums
{
    public enum AchievementCategoryType : byte
    {
        FirstSteps = 1,
        Gathering = 2,
        Fishing = 3,
        Harvesting = 4,
        Cooking = 5,
        Crafting = 6,
        Trading = 7,
        Casino = 8,
        Collection = 9,
        Event = 10
    }

    public static class AchievementCategoryHelper
    {
        public static string Localize(this AchievementCategoryType category) => category switch
        {
            AchievementCategoryType.FirstSteps => "Первые шаги",
            AchievementCategoryType.Gathering => "Собирательство",
            AchievementCategoryType.Fishing => "Рыбалка",
            AchievementCategoryType.Harvesting => "Урожай",
            AchievementCategoryType.Cooking => "Кулинария",
            AchievementCategoryType.Crafting => "Изготовление",
            AchievementCategoryType.Trading => "Торговля",
            AchievementCategoryType.Casino => "Казино",
            AchievementCategoryType.Collection => "Коллекция",
            AchievementCategoryType.Event => "События",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
