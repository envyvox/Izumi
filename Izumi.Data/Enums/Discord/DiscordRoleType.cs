using System;

namespace Izumi.Data.Enums.Discord
{
    public enum DiscordRoleType : byte
    {
        MusicBot = 1,
        Administration = 2,
        EventManager = 3,
        Moderator = 4,
        Nitro = 5, // роль nitro-boost создается дискордом по-умолчанию, нам нужно только получить ее
        Mute = 6,
        ContentProvider = 7,
        InVoice = 8,
        Premium = 9,
        DiscordEvent = 10,

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

        GenshinImpact = 200,
        LeagueOfLegends = 201,
        TeamfightTactics = 202,
        Valorant = 203,
        ApexLegends = 204,
        LostArk = 205,
        Dota = 206,
        Osu = 207,
        AmongUs = 208,
        Rust = 209,
        CsGo = 210,
        HotS = 211,
        WildRift = 212,
        MobileLegends = 213
    }

    public static class DiscordRoleHelper
    {
        public static string Name(this DiscordRoleType role) => role switch
        {
            DiscordRoleType.MusicBot => "Музыкальные боты",
            DiscordRoleType.Administration => "Сёгунат",
            DiscordRoleType.EventManager => "Собаёри",
            DiscordRoleType.Moderator => "Родзю",
            DiscordRoleType.Nitro => "🤝 Поддержка сервера",
            DiscordRoleType.Premium => "👑 Премиум",
            DiscordRoleType.DiscordEvent => "🥳 Мероприятия",
            DiscordRoleType.LocationInTransit => "🐫 " + LocationType.InTransit.Localize(),
            DiscordRoleType.LocationCapital => "🏯 " + LocationType.Capital.Localize(),
            DiscordRoleType.LocationGarden => "🌳 " + LocationType.Garden.Localize(),
            DiscordRoleType.LocationSeaport => "⛵ " + LocationType.Seaport.Localize(),
            DiscordRoleType.LocationCastle => "🏰 " + LocationType.Castle.Localize(),
            DiscordRoleType.LocationVillage => "🎑 " + LocationType.Village.Localize(),
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
            DiscordRoleType.LostArk => "LostArk",
            DiscordRoleType.Dota => "Dota 2",
            DiscordRoleType.Osu => "Osu!",
            DiscordRoleType.AmongUs => "Among Us",
            DiscordRoleType.Mute => "Блокировка чата",
            DiscordRoleType.ContentProvider => "❤️ Поставщик контента",
            DiscordRoleType.InVoice => "🎙️",
            DiscordRoleType.Rust => "Rust",
            DiscordRoleType.CsGo => "CSGO",
            DiscordRoleType.HotS => "HotS",
            DiscordRoleType.WildRift => "Wild Rift",
            DiscordRoleType.MobileLegends => "Mobile Legends",
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
            // для всех остальных используем значение по-умолчанию (прозрачный цвет дискорда)
            _ => "000000"
        };
    }
}
