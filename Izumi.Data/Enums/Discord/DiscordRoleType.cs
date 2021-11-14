using System;

namespace Izumi.Data.Enums.Discord
{
    public enum DiscordRoleType : byte
    {
        Muted,

        Administration,
        EventManager,
        Moderator,

        Premium,
        Streamer,
        Nitro, // nitro-boost role are created by discord, we only need to get it
        Creative,
        Friends,

        ContentProvider,
        Active,

        Newbie,
        Lucky,
        ResourcefulCatcher,
        DescendantAristocracy,
        DescendantOcean,
        KeeperGrove,
        ReliableWorkaholic,
        SereneExcavator,
        AgileEarner,
        Handyman,
        WineSamurai,
        StockyFarmer,
        SeaPoet,
        CulinaryIdol,
        Toxic,
        KingExcitement,
        BelievingInLuck,
        FirstSamurai,
        Yatagarasu,
        HarbingerOfSummer,
        DrinkCollection,

        GenderMale,
        GenderFemale,

        LocationInTransit,
        LocationCapital,
        LocationGarden,
        LocationSeaport,
        LocationCastle,
        LocationVillage,

        GenshinImpact,
        LeagueOfLegends,
        TeamfightTactics,
        Valorant,
        ApexLegends,
        Dota,
        Minecraft,
        Osu,
        AmongUs,
        Rust,
        CsGo,
        HotS,
        NewWorld,
        MobileGaming,

        InVoice
    }

    public static class DiscordRoleHelper
    {
        public static string Name(this DiscordRoleType role) => role switch
        {
            DiscordRoleType.Muted => "Блокировка чата",

            DiscordRoleType.Administration => "Администраторы",
            DiscordRoleType.EventManager => "Организаторы",
            DiscordRoleType.Moderator => "Модераторы",

            DiscordRoleType.Premium => "Премиум",
            DiscordRoleType.Streamer => "Стримеры",
            DiscordRoleType.Nitro => "Поддержка сервера",
            DiscordRoleType.Creative => "Креативный вклад",
            DiscordRoleType.Friends => "Друзья проекта",

            DiscordRoleType.ContentProvider => "Поставщик контента",
            DiscordRoleType.Active => "Активные",

            DiscordRoleType.Newbie => TitleType.Newbie.Localize(),
            DiscordRoleType.Lucky => TitleType.Lucky.Localize(),
            DiscordRoleType.ResourcefulCatcher => TitleType.ResourcefulCatcher.Localize(),
            DiscordRoleType.DescendantAristocracy => TitleType.DescendantAristocracy.Localize(),
            DiscordRoleType.DescendantOcean => TitleType.DescendantOcean.Localize(),
            DiscordRoleType.KeeperGrove => TitleType.KeeperGrove.Localize(),
            DiscordRoleType.ReliableWorkaholic => TitleType.ReliableWorkaholic.Localize(),
            DiscordRoleType.SereneExcavator => TitleType.SereneExcavator.Localize(),
            DiscordRoleType.AgileEarner => TitleType.AgileEarner.Localize(),
            DiscordRoleType.Handyman => TitleType.Handyman.Localize(),
            DiscordRoleType.WineSamurai => TitleType.WineSamurai.Localize(),
            DiscordRoleType.StockyFarmer => TitleType.StockyFarmer.Localize(),
            DiscordRoleType.SeaPoet => TitleType.SeaPoet.Localize(),
            DiscordRoleType.CulinaryIdol => TitleType.CulinaryIdol.Localize(),
            DiscordRoleType.Toxic => TitleType.Toxic.Localize(),
            DiscordRoleType.KingExcitement => TitleType.KingExcitement.Localize(),
            DiscordRoleType.BelievingInLuck => TitleType.BelievingInLuck.Localize(),
            DiscordRoleType.FirstSamurai => TitleType.FirstSamurai.Localize(),
            DiscordRoleType.Yatagarasu => TitleType.Yatagarasu.Localize(),
            DiscordRoleType.HarbingerOfSummer => TitleType.HarbingerOfSummer.Localize(),
            DiscordRoleType.DrinkCollection => TitleType.DrinkCollection.Localize(),

            DiscordRoleType.GenderMale => "Оками",
            DiscordRoleType.GenderFemale => "Китсунэ",

            DiscordRoleType.LocationInTransit => LocationType.InTransit.Localize(),
            DiscordRoleType.LocationCapital => LocationType.Capital.Localize(),
            DiscordRoleType.LocationGarden => LocationType.Garden.Localize(),
            DiscordRoleType.LocationSeaport => LocationType.Seaport.Localize(),
            DiscordRoleType.LocationCastle => LocationType.Castle.Localize(),
            DiscordRoleType.LocationVillage => LocationType.Village.Localize(),

            DiscordRoleType.GenshinImpact => "Genshin Impact",
            DiscordRoleType.LeagueOfLegends => "League of Legends",
            DiscordRoleType.TeamfightTactics => "Teamfight Tactics",
            DiscordRoleType.Valorant => "Valorant",
            DiscordRoleType.ApexLegends => "Apex Legends",
            DiscordRoleType.Dota => "Dota 2",
            DiscordRoleType.Minecraft => "Minecraft",
            DiscordRoleType.Osu => "Osu!",
            DiscordRoleType.AmongUs => "Among Us",
            DiscordRoleType.Rust => "Rust",
            DiscordRoleType.CsGo => "CSGO",
            DiscordRoleType.HotS => "HotS",
            DiscordRoleType.NewWorld => "New World",
            DiscordRoleType.MobileGaming => "Mobile Gaming",


            DiscordRoleType.InVoice => "🎙️",
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };

        public static string Color(this DiscordRoleType role) => role switch
        {
            DiscordRoleType.Administration => "ffc7f5",
            DiscordRoleType.EventManager => "e99edb",
            DiscordRoleType.Moderator => "c072b2",
            DiscordRoleType.Nitro => "f47fff",
            DiscordRoleType.ContentProvider => "6fffc4",
            DiscordRoleType.Premium => "ffb71d",
            DiscordRoleType.Newbie => "8ecbf1",
            DiscordRoleType.Lucky => "9741b8",
            DiscordRoleType.ResourcefulCatcher => "f0c3c3",
            DiscordRoleType.DescendantAristocracy => "631515",
            DiscordRoleType.DescendantOcean => "385e9b",
            DiscordRoleType.KeeperGrove => "5be274",
            DiscordRoleType.ReliableWorkaholic => "000000", // todo change value
            DiscordRoleType.SereneExcavator => "ffbb88",
            DiscordRoleType.AgileEarner => "f3bc6a",
            DiscordRoleType.Handyman => "914f1e",
            DiscordRoleType.WineSamurai => "a32121",
            DiscordRoleType.StockyFarmer => "36d451",
            DiscordRoleType.SeaPoet => "72abff",
            DiscordRoleType.CulinaryIdol => "f3ff38",
            DiscordRoleType.Toxic => "000000", // todo change value
            DiscordRoleType.KingExcitement => "000000", // todo change value
            DiscordRoleType.BelievingInLuck => "2c8f24",
            DiscordRoleType.FirstSamurai => "050000",
            DiscordRoleType.Yatagarasu => "ff1901",
            DiscordRoleType.HarbingerOfSummer => "ebf0ac",
            DiscordRoleType.DrinkCollection => "000000", // todo change value
            DiscordRoleType.GenderMale => "5ca5f9",
            DiscordRoleType.GenderFemale => "ff7799",
            // для всех остальных используем значение по-умолчанию (прозрачный цвет дискорда)
            _ => "000000"
        };
    }
}