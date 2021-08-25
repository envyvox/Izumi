using System;

namespace Izumi.Data.Enums
{
    public enum TitleType : byte
    {
        Newbie = 1,
        Lucky = 2,
        ResourcefulCatcher = 3,
        DescendantAristocracy = 4,
        DescendantOcean = 5,
        KeeperGrove = 6,
        ReliableWorkaholic = 7,
        SereneExcavator = 8,
        AgileEarner = 9,
        Handyman = 10,
        WineSamurai = 11,
        StockyFarmer = 12,
        SeaPoet = 13,
        CulinaryIdol = 14,
        Toxic = 15,
        KingExcitement = 16,
        BelievingInLuck = 17,
        FirstSamurai = 18,
        Yatagarasu = 19,
        HarbingerOfSummer = 20,
        Wanderer = 255 // титул для Изуми
    }

    public static class TitleHelper
    {
        public static string Localize(this TitleType title) => title switch
        {
            TitleType.Newbie => "Новичок",
            TitleType.Lucky => "Приносящий удачу",
            TitleType.ResourcefulCatcher => "Находчивый ловец",
            TitleType.DescendantAristocracy => "Потомок аристократии",
            TitleType.DescendantOcean => "Потомок океана",
            TitleType.KeeperGrove => "Хранитель рощи",
            TitleType.ReliableWorkaholic => "Надежный трудяга",
            TitleType.SereneExcavator => "Безмятежный землекоп",
            TitleType.AgileEarner => "Проворный добытчик",
            TitleType.Handyman => "Мастер на все руки",
            TitleType.WineSamurai => "Винный самурай",
            TitleType.StockyFarmer => "Запасливый фермер",
            TitleType.SeaPoet => "Морской поэт",
            TitleType.CulinaryIdol => "Кулинарный идол",
            TitleType.Toxic => "Токсичный",
            TitleType.KingExcitement => "Король азарта",
            TitleType.BelievingInLuck => "Верящий в удачу",
            TitleType.Wanderer => "Странница",
            TitleType.FirstSamurai => "Первый самурай",
            TitleType.Yatagarasu => "Ятагарасу",
            TitleType.HarbingerOfSummer => "Предвестник лета",
            _ => throw new ArgumentOutOfRangeException(nameof(title), title, null)
        };

        public static string EmoteName(this TitleType title) => "Title" + title;
    }
}
