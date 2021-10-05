using System;
using Izumi.Data.Enums.Discord;

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
        DrinkCollection = 21,
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
            TitleType.DrinkCollection => "Титул за коллекцию напитков",
            _ => throw new ArgumentOutOfRangeException(nameof(title), title, null)
        };

        public static string EmoteName(this TitleType title) => "Title" + title;

        public static DiscordRoleType Role(this TitleType title) => title switch
        {
            TitleType.Newbie => DiscordRoleType.Newbie,
            TitleType.Lucky => DiscordRoleType.Lucky,
            TitleType.ResourcefulCatcher => DiscordRoleType.ResourcefulCatcher,
            TitleType.DescendantAristocracy => DiscordRoleType.DescendantAristocracy,
            TitleType.DescendantOcean => DiscordRoleType.DescendantOcean,
            TitleType.KeeperGrove => DiscordRoleType.KeeperGrove,
            TitleType.ReliableWorkaholic => DiscordRoleType.ReliableWorkaholic,
            TitleType.SereneExcavator => DiscordRoleType.SereneExcavator,
            TitleType.AgileEarner => DiscordRoleType.AgileEarner,
            TitleType.Handyman => DiscordRoleType.Handyman,
            TitleType.WineSamurai => DiscordRoleType.WineSamurai,
            TitleType.StockyFarmer => DiscordRoleType.StockyFarmer,
            TitleType.SeaPoet => DiscordRoleType.SeaPoet,
            TitleType.CulinaryIdol => DiscordRoleType.CulinaryIdol,
            TitleType.Toxic => DiscordRoleType.Toxic,
            TitleType.KingExcitement => DiscordRoleType.KingExcitement,
            TitleType.BelievingInLuck => DiscordRoleType.BelievingInLuck,
            TitleType.FirstSamurai => DiscordRoleType.FirstSamurai,
            TitleType.Yatagarasu => DiscordRoleType.Yatagarasu,
            TitleType.HarbingerOfSummer => DiscordRoleType.HarbingerOfSummer,
            TitleType.DrinkCollection => DiscordRoleType.DrinkCollection,
            _ => throw new ArgumentOutOfRangeException(nameof(title), title, null)
        };
    }
}
