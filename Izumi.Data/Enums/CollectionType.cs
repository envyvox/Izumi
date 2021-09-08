using System;

namespace Izumi.Data.Enums
{
    public enum CollectionType : byte
    {
        Gathering = 1,
        Crafting = 2,
        Alcohol = 3,
        Drink = 4,
        Crop = 5,
        Fish = 6,
        Food = 7
    }

    public static class CollectionHelper
    {
        public static string Localize(this CollectionType collection) => collection switch
        {
            CollectionType.Gathering => "Собительские предметы",
            CollectionType.Crafting => "Изготавливаемые предметы",
            CollectionType.Alcohol => "Алкоголь",
            CollectionType.Drink => "Напитки",
            CollectionType.Crop => "Урожай",
            CollectionType.Fish => "Рыба",
            CollectionType.Food => "Блюда",
            _ => throw new ArgumentOutOfRangeException(nameof(collection), collection, null)
        };
    }
}
