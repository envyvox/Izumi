using System;

namespace Izumi.Data.Enums.Discord
{
    public enum DiscordChannelType : byte
    {
        Chat = 1,
        Commands = 2,

        SearchParent = 3,
        SearchGetRoles = 4,
        SearchGenshinImpact = 5,
        SearchLeagueOfLegends = 6,
        SearchTeamfightTactics = 7,
        SearchValorant = 8,
        SearchApexLegends = 9,
        SearchLostArk = 10,
        SearchDota = 11,
        SearchOsu = 12,
        SearchAmongUs = 13,
        SearchRust = 14,
        SearchCsGo = 15,
        SearchHotS = 16,
        SearchWildRift = 17,
        SearchMobileLegends = 18,
        SearchNewWorld = 19,

        EventParent = 20,
        EventGetRoles = 21,
        EventNotification = 22,
        EventCreateRoom = 23,

        GameParent = 24,
        GameStart = 25,
        GameUpdates = 26,
        GameDiary = 27,

        CommunityDescParent = 28,
        CommunityDescHowItWork = 29,
        Photos = 30,
        Screenshots = 31,
        Memes = 32,
        Arts = 33,
        Erotic = 34,
        Nsfw = 35,

        LibraryParent = 36,
        Rules = 37,
        Announcements = 38,
        Giveaways = 39,
        Suggestions = 40,

        CreateRoomParent = 41,
        NoMic = 42,
        CreateRoom = 43,

        FamilyRoomParent = 44,

        CapitalParent = 45,
        CapitalDesc = 46,
        CapitalEvents = 48,

        GardenParent = 49,
        GardenDesc = 50,
        GardenEvents = 52,

        SeaportParent = 53,
        SeaportDesc = 54,
        SeaportEvents = 56,

        CastleParent = 57,
        CastleDesc = 58,
        CastleEvents = 60,

        VillageParent = 61,
        VillageDesc = 62,
        VillageEvents = 64,

        AfkParent = 65,
        Afk = 66,

        AdministrationParent = 67,
        Administration = 68,
        Moderation = 69,
        EventManager = 70,
        Meeting = 71
    }

    public static class DiscordChannelHelper
    {
        private const string LocationDesc = "описание";
        private const string LocationEvents = "события";

        public static string Name(this DiscordChannelType channel) => channel switch
        {
            DiscordChannelType.Chat => "общение",
            DiscordChannelType.Commands => "команды",

            DiscordChannelType.SearchParent => "поиск игроков",
            DiscordChannelType.SearchGetRoles => "получение-ролей",
            DiscordChannelType.SearchGenshinImpact => "genshin-impact",
            DiscordChannelType.SearchLeagueOfLegends => "league-of-legends",
            DiscordChannelType.SearchTeamfightTactics => "teamfight-tactics",
            DiscordChannelType.SearchValorant => "valorant",
            DiscordChannelType.SearchApexLegends => "apex-legends",
            DiscordChannelType.SearchLostArk => "lost-ark",
            DiscordChannelType.SearchDota => "dota",
            DiscordChannelType.SearchAmongUs => "among-us",
            DiscordChannelType.SearchOsu => "osu",
            DiscordChannelType.SearchRust => "rust",
            DiscordChannelType.SearchCsGo => "cs-go",
            DiscordChannelType.SearchHotS => "hots",
            DiscordChannelType.SearchWildRift => "wild-rift",
            DiscordChannelType.SearchMobileLegends => "mobile-legends",
            DiscordChannelType.SearchNewWorld => "new-world",

            DiscordChannelType.EventParent => "мероприятия",
            DiscordChannelType.EventGetRoles => "получение-роли",
            DiscordChannelType.EventNotification => "оповещения🔔",
            DiscordChannelType.EventCreateRoom => "Начать мероприятие",

            DiscordChannelType.GameParent => "игровая вселенная",
            DiscordChannelType.GameStart => "информация",
            DiscordChannelType.GameUpdates => "обновления🔔",
            DiscordChannelType.GameDiary => "дневник-странницы",

            DiscordChannelType.CommunityDescParent => "доска сообщества",
            DiscordChannelType.CommunityDescHowItWork => "как-работает",
            DiscordChannelType.Photos => "фотографии",
            DiscordChannelType.Screenshots => "скриншоты",
            DiscordChannelType.Memes => "мемесы",
            DiscordChannelType.Arts => "арты",
            DiscordChannelType.Erotic => "эротика",
            DiscordChannelType.Nsfw => "nsfw",

            DiscordChannelType.LibraryParent => "великая «тосёкан»",
            DiscordChannelType.Rules => "правила",
            DiscordChannelType.Announcements => "объявления🔔",
            DiscordChannelType.Giveaways => "розыгрыши🔔",
            DiscordChannelType.Suggestions => "предложения",

            DiscordChannelType.CreateRoomParent => "пригородные лагеря",
            DiscordChannelType.NoMic => "без-микрофона",
            DiscordChannelType.CreateRoom => "Разжечь костер",

            DiscordChannelType.FamilyRoomParent => "семейные беседки",

            DiscordChannelType.CapitalParent => LocationType.Capital.Localize(),
            DiscordChannelType.CapitalDesc => "🏯" + LocationDesc,
            DiscordChannelType.CapitalEvents => "🏯" + LocationEvents,

            DiscordChannelType.GardenParent => LocationType.Garden.Localize(),
            DiscordChannelType.GardenDesc => "🌳" + LocationDesc,
            DiscordChannelType.GardenEvents => "🌳" + LocationEvents,

            DiscordChannelType.SeaportParent => LocationType.Seaport.Localize(),
            DiscordChannelType.SeaportDesc => "⛵" + LocationDesc,
            DiscordChannelType.SeaportEvents => "⛵" + LocationEvents,

            DiscordChannelType.CastleParent => LocationType.Castle.Localize(),
            DiscordChannelType.CastleDesc => "🏰" + LocationDesc,
            DiscordChannelType.CastleEvents => "🏰" + LocationEvents,

            DiscordChannelType.VillageParent => LocationType.Village.Localize(),
            DiscordChannelType.VillageDesc => "🎑" + LocationDesc,
            DiscordChannelType.VillageEvents => "🎑" + LocationEvents,

            DiscordChannelType.AfkParent => "zzz",
            DiscordChannelType.Afk => "Афк, жду подарки",

            DiscordChannelType.AdministrationParent => "скрытый раздел",
            DiscordChannelType.Administration => "сёгунат",
            DiscordChannelType.Moderation => "родзю",
            DiscordChannelType.EventManager => "собаёри",
            DiscordChannelType.Meeting => "Собрание",

            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

        public static DiscordChannelCategoryType Category(this DiscordChannelType channel) => channel switch
        {
            DiscordChannelType.Chat => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Commands => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.SearchParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.SearchGetRoles => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchGenshinImpact => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchLeagueOfLegends => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchTeamfightTactics => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchValorant => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchApexLegends => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchLostArk => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchDota => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchAmongUs => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchOsu => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchRust => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchCsGo => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchHotS => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchWildRift => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchMobileLegends => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchNewWorld => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.EventParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.EventGetRoles => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.EventNotification => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.EventCreateRoom => DiscordChannelCategoryType.VoiceChannel,

            DiscordChannelType.GameParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.GameStart => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.GameUpdates => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.GameDiary => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.CommunityDescParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.CommunityDescHowItWork => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Photos => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Screenshots => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Memes => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Arts => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Erotic => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Nsfw => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.LibraryParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.Rules => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Announcements => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Giveaways => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Suggestions => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.CreateRoomParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.NoMic => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.CreateRoom => DiscordChannelCategoryType.VoiceChannel,

            DiscordChannelType.FamilyRoomParent => DiscordChannelCategoryType.CategoryChannel,

            DiscordChannelType.CapitalParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.CapitalDesc => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.CapitalEvents => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.GardenParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.GardenDesc => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.GardenEvents => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.SeaportParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.SeaportDesc => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SeaportEvents => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.CastleParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.CastleDesc => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.CastleEvents => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.VillageParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.VillageDesc => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.VillageEvents => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.AfkParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.Afk => DiscordChannelCategoryType.VoiceChannel,

            DiscordChannelType.AdministrationParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.Administration => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Moderation => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.EventManager => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Meeting => DiscordChannelCategoryType.VoiceChannel,

            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

        public static DiscordChannelType Parent(this DiscordChannelType channel) => channel switch
        {
            DiscordChannelType.SearchGetRoles => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchGenshinImpact => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchLeagueOfLegends => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchTeamfightTactics => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchValorant => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchApexLegends => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchLostArk => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchDota => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchOsu => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchAmongUs => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchRust => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchHotS => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchWildRift => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchMobileLegends => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchNewWorld => DiscordChannelType.SearchParent,

            DiscordChannelType.EventGetRoles => DiscordChannelType.EventParent,
            DiscordChannelType.EventNotification => DiscordChannelType.EventParent,
            DiscordChannelType.EventCreateRoom => DiscordChannelType.EventParent,

            DiscordChannelType.GameStart => DiscordChannelType.GameParent,
            DiscordChannelType.GameUpdates => DiscordChannelType.GameParent,
            DiscordChannelType.GameDiary => DiscordChannelType.GameParent,

            DiscordChannelType.CommunityDescHowItWork => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Photos => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Screenshots => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Memes => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Arts => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Erotic => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Nsfw => DiscordChannelType.CommunityDescParent,

            DiscordChannelType.Rules => DiscordChannelType.LibraryParent,
            DiscordChannelType.Announcements => DiscordChannelType.LibraryParent,
            DiscordChannelType.Giveaways => DiscordChannelType.LibraryParent,
            DiscordChannelType.Suggestions => DiscordChannelType.LibraryParent,

            DiscordChannelType.NoMic => DiscordChannelType.CreateRoomParent,
            DiscordChannelType.CreateRoom => DiscordChannelType.CreateRoomParent,

            DiscordChannelType.CapitalDesc => DiscordChannelType.CapitalParent,
            DiscordChannelType.CapitalEvents => DiscordChannelType.CapitalParent,

            DiscordChannelType.GardenDesc => DiscordChannelType.GardenParent,
            DiscordChannelType.GardenEvents => DiscordChannelType.GardenParent,

            DiscordChannelType.SeaportDesc => DiscordChannelType.SeaportParent,
            DiscordChannelType.SeaportEvents => DiscordChannelType.SeaportParent,

            DiscordChannelType.CastleDesc => DiscordChannelType.CastleParent,
            DiscordChannelType.CastleEvents => DiscordChannelType.CastleParent,

            DiscordChannelType.VillageDesc => DiscordChannelType.VillageParent,
            DiscordChannelType.VillageEvents => DiscordChannelType.VillageParent,

            DiscordChannelType.Afk => DiscordChannelType.AfkParent,

            DiscordChannelType.Administration => DiscordChannelType.AdministrationParent,
            DiscordChannelType.Moderation => DiscordChannelType.AdministrationParent,
            DiscordChannelType.EventManager => DiscordChannelType.AdministrationParent,
            DiscordChannelType.Meeting => DiscordChannelType.AdministrationParent,

            _ => channel
        };
    }
}
