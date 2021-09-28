using System;

namespace Izumi.Data.Enums
{
    public enum BuildingCategoryType : byte
    {
        Personal = 1,
        Family = 2
    }

    public static class BuildingCategoryHelper
    {
        public static string Localize(this BuildingCategoryType category, bool declension = false) => category switch
        {
            BuildingCategoryType.Personal => declension ? "Персональный" : "Персональные",
            BuildingCategoryType.Family => declension ? "Семейный" : "Семейные",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
