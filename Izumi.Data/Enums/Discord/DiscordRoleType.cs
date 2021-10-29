﻿using System;

namespace Izumi.Data.Enums.Discord
{
    public enum DiscordRoleType : byte
    {
        Administration = 2,
        EventManager = 3,
        Moderator = 4,
        Nitro = 5, // роль nitro-boost создается дискордом по-умолчанию, нам нужно только получить ее
        Muted = 6,
        ContentProvider = 7,
        InVoice = 8,
        Premium = 9,
        DiscordEvent = 10,
        GenderMale = 11,
        GenderFemale = 12,

        LocationInTransit = 50,
        LocationCapital = 51,
        LocationGarden = 52,
        LocationSeaport = 53,
        LocationCastle = 54,
        LocationVillage = 55,

        AllEvents = 100,
        DailyEvents = 101,
        WeeklyEvents = 102,
        MonthlyEvents = 103,
        YearlyEvents = 104,
        UniqueEvents = 105,

        Newbie = 151,
        Lucky = 152,
        ResourcefulCatcher = 153,
        DescendantAristocracy = 154,
        DescendantOcean = 155,
        KeeperGrove = 156,
        ReliableWorkaholic = 157,
        SereneExcavator = 158,
        AgileEarner = 159,
        Handyman = 160,
        WineSamurai = 161,
        StockyFarmer = 162,
        SeaPoet = 163,
        CulinaryIdol = 164,
        Toxic = 165,
        KingExcitement = 166,
        BelievingInLuck = 167,
        FirstSamurai = 168,
        Yatagarasu = 169,
        HarbingerOfSummer = 170,
        DrinkCollection = 171,

        GenshinImpact = 200,
        LeagueOfLegends = 201,
        TeamfightTactics = 202,
        Valorant = 203,
        ApexLegends = 204,
        Dota = 205,
        Minecraft = 206,
        Osu = 207,
        AmongUs = 208,
        Rust = 209,
        CsGo = 210,
        HotS = 211,
        NewWorld = 212,
        MobileGaming = 213
    }

    public static class DiscordRoleHelper
    {
        public static string Name(this DiscordRoleType role) => role switch
        {
            DiscordRoleType.Administration => "Администраторы",
            DiscordRoleType.EventManager => "Организаторы",
            DiscordRoleType.Moderator => "Модераторы",
            DiscordRoleType.Nitro => "Поддержка сервера",
            DiscordRoleType.Premium => "Премиум",
            DiscordRoleType.DiscordEvent => "🥳 Мероприятия",
            DiscordRoleType.GenderMale => "Оками",
            DiscordRoleType.GenderFemale => "Китсунэ",
            DiscordRoleType.LocationInTransit => LocationType.InTransit.Localize(),
            DiscordRoleType.LocationCapital => LocationType.Capital.Localize(),
            DiscordRoleType.LocationGarden => LocationType.Garden.Localize(),
            DiscordRoleType.LocationSeaport => LocationType.Seaport.Localize(),
            DiscordRoleType.LocationCastle => LocationType.Castle.Localize(),
            DiscordRoleType.LocationVillage => LocationType.Village.Localize(),
            DiscordRoleType.AllEvents => "📅 Все события",
            DiscordRoleType.DailyEvents => "📅 Ежедневные события",
            DiscordRoleType.WeeklyEvents => "📅 Еженедельные события",
            DiscordRoleType.MonthlyEvents => "📅 Ежемесячные события",
            DiscordRoleType.YearlyEvents => "📅 Ежегодные события",
            DiscordRoleType.UniqueEvents => "📅 Уникальные события",
            DiscordRoleType.GenshinImpact => "Genshin Impact",
            DiscordRoleType.LeagueOfLegends => "League of Legends",
            DiscordRoleType.TeamfightTactics => "Teamfight Tactics",
            DiscordRoleType.Valorant => "Valorant",
            DiscordRoleType.ApexLegends => "Apex Legends",
            DiscordRoleType.Dota => "Dota 2",
            DiscordRoleType.Minecraft => "Minecraft",
            DiscordRoleType.Osu => "Osu!",
            DiscordRoleType.AmongUs => "Among Us",
            DiscordRoleType.Muted => "Блокировка чата",
            DiscordRoleType.ContentProvider => "Поставщик контента",
            DiscordRoleType.InVoice => "🎙️",
            DiscordRoleType.Rust => "Rust",
            DiscordRoleType.CsGo => "CSGO",
            DiscordRoleType.HotS => "HotS",
            DiscordRoleType.NewWorld => "New World",
            DiscordRoleType.MobileGaming => "Mobile Gaming",
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
