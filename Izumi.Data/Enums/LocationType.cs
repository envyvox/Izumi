using System;
using Izumi.Data.Enums.Discord;

namespace Izumi.Data.Enums
{
    public enum LocationType : byte
    {
        InTransit = 1,
        Capital = 2,
        Garden = 3,
        Seaport = 4,
        Castle = 5,
        Village = 6,
        ExploreGarden = 7,
        ExploreCastle = 8,
        Fishing = 9,
        FieldWatering = 10,
        WorkOnContract = 11,
        CraftingResource = 12,
        CraftingAlcohol = 13,
        CraftingDrink = 14,
        CraftingFood = 15
    }

    public static class LocationHelper
    {
        public static string Localize(this LocationType location, bool declension = false) => location switch
        {
            LocationType.InTransit => declension ? "пути" : "В пути",
            LocationType.Capital => declension ? "столице «Эдо»" : "Столица «Эдо»",
            LocationType.Garden => declension ? "цветущем саду «Кайраку-эн»" : "Цветущий сад «Кайраку-эн»",
            LocationType.Seaport => declension ? "портовом городе «Нагоя»" : "Портовый город «Нагоя»",
            LocationType.Castle => declension ? "древнем замке «Химэдзи»" : "Древний замок «Химэдзи»",
            LocationType.Village => declension ? "деревне «Мура»" : "Деревня «Мура»",
            LocationType.ExploreGarden => declension ? "исследовании сада" : "Исследование сада",
            LocationType.ExploreCastle => declension ? "исследовании шахт" : "Исследование шахт",
            LocationType.Fishing => declension ? "рыбалке" : "Рыбалка",
            LocationType.FieldWatering => declension ? "поливке участка земли" : "Поливка участка земли",
            LocationType.WorkOnContract => declension
                ? "."
                : "..", // Вместо названия локации выводится название контракта
            LocationType.CraftingResource => declension ? "изготовлении предметов" : "Изготовление предметов",
            LocationType.CraftingAlcohol => declension ? "изготовлении алкоголя" : "Изготовление алкоголя",
            LocationType.CraftingDrink => declension ? "изготовлении напитков" : "Изготовление напитков",
            LocationType.CraftingFood => declension ? "изготовлении блюда" : "Изготовление блюда",
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };

        public static DiscordRoleType Role(this LocationType location) => location switch
        {
            LocationType.InTransit => DiscordRoleType.LocationInTransit,
            LocationType.Capital => DiscordRoleType.LocationCapital,
            LocationType.Garden => DiscordRoleType.LocationGarden,
            LocationType.Seaport => DiscordRoleType.LocationSeaport,
            LocationType.Castle => DiscordRoleType.LocationCastle,
            LocationType.Village => DiscordRoleType.LocationVillage,
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };

        public static ReputationType Reputation(this LocationType location) => location switch
        {
            LocationType.Capital => ReputationType.Capital,
            LocationType.Garden => ReputationType.Garden,
            LocationType.Seaport => ReputationType.Seaport,
            LocationType.Castle => ReputationType.Castle,
            LocationType.Village => ReputationType.Village,
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };
    }
}
