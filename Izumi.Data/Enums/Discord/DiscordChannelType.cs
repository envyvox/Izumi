using System;

namespace Izumi.Data.Enums.Discord
{
    public enum DiscordChannelType : byte
    {
        Chat = 1,
        Commands = 2,
        GetRoles = 4,

        SearchParent = 3,
        SearchGenshinImpact = 5,
        SearchLeagueOfLegends = 6,
        SearchTeamfightTactics = 7,
        SearchValorant = 8,
        SearchApexLegends = 9,
        SearchDota = 10,
        SearchMinecraft = 11,
        SearchOsu = 12,
        SearchAmongUs = 13,
        SearchRust = 14,
        SearchCsGo = 15,
        SearchHotS = 16,
        SearchNewWorld = 17,
        SearchMobileGaming = 18,

        EventParent = 20,
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
        Staff = 72,
        Meeting = 71
    }

    public static class DiscordChannelHelper
    {
        private const string Emote = "・";
        private const string LocationDesc = "описание";
        private const string LocationEvents = "события";

        public static string Name(this DiscordChannelType channel) => channel switch
        {
            DiscordChannelType.Chat => Emote + "общение",
            DiscordChannelType.Commands => Emote + "команды",
            DiscordChannelType.GetRoles => Emote + "получение-ролей",

            DiscordChannelType.SearchParent => "поиск игроков",
            DiscordChannelType.SearchGenshinImpact => Emote + "genshin-impact",
            DiscordChannelType.SearchLeagueOfLegends => Emote + "league-of-legends",
            DiscordChannelType.SearchTeamfightTactics => Emote + "teamfight-tactics",
            DiscordChannelType.SearchValorant => Emote + "valorant",
            DiscordChannelType.SearchApexLegends => Emote + "apex-legends",
            DiscordChannelType.SearchDota => Emote + "dota",
            DiscordChannelType.SearchMinecraft => Emote + "minecraft",
            DiscordChannelType.SearchAmongUs => Emote + "among-us",
            DiscordChannelType.SearchOsu => Emote + "osu",
            DiscordChannelType.SearchRust => Emote + "rust",
            DiscordChannelType.SearchCsGo => Emote + "cs-go",
            DiscordChannelType.SearchHotS => Emote + "hots",
            DiscordChannelType.SearchNewWorld => Emote + "new-world",
            DiscordChannelType.SearchMobileGaming => Emote + "mobile-gaming",

            DiscordChannelType.EventParent => "мероприятия",
            DiscordChannelType.EventNotification => Emote + "оповещения🔔",
            DiscordChannelType.EventCreateRoom => "Начать мероприятие",

            DiscordChannelType.GameParent => "игровая вселенная",
            DiscordChannelType.GameStart => Emote + "информация",
            DiscordChannelType.GameUpdates => Emote + "обновления🔔",
            DiscordChannelType.GameDiary => Emote + "дневник-странницы",

            DiscordChannelType.CommunityDescParent => "доска сообщества",
            DiscordChannelType.CommunityDescHowItWork => Emote + "как-работает",
            DiscordChannelType.Photos => Emote + "фотографии",
            DiscordChannelType.Screenshots => Emote + "скриншоты",
            DiscordChannelType.Memes => Emote + "мемесы",
            DiscordChannelType.Arts => Emote + "арты",
            DiscordChannelType.Erotic => Emote + "эротика",
            DiscordChannelType.Nsfw => Emote + "nsfw",

            DiscordChannelType.LibraryParent => "великая «тосёкан»",
            DiscordChannelType.Rules => Emote + "правила",
            DiscordChannelType.Announcements => Emote + "объявления🔔",
            DiscordChannelType.Giveaways => Emote + "розыгрыши🔔",
            DiscordChannelType.Suggestions => Emote + "предложения",

            DiscordChannelType.CreateRoomParent => "пригородные лагеря",
            DiscordChannelType.NoMic => Emote + "без-микрофона",
            DiscordChannelType.CreateRoom => "Разжечь костер",

            DiscordChannelType.FamilyRoomParent => "семейные беседки",

            DiscordChannelType.CapitalParent => LocationType.Capital.Localize(),
            DiscordChannelType.CapitalDesc => "🏯" + Emote + LocationDesc,
            DiscordChannelType.CapitalEvents => "🏯" + Emote + LocationEvents,

            DiscordChannelType.GardenParent => LocationType.Garden.Localize(),
            DiscordChannelType.GardenDesc => "🌳" + Emote + LocationDesc,
            DiscordChannelType.GardenEvents => "🌳" + Emote + LocationEvents,

            DiscordChannelType.SeaportParent => LocationType.Seaport.Localize(),
            DiscordChannelType.SeaportDesc => "⛵" + Emote + LocationDesc,
            DiscordChannelType.SeaportEvents => "⛵" + Emote + LocationEvents,

            DiscordChannelType.CastleParent => LocationType.Castle.Localize(),
            DiscordChannelType.CastleDesc => "🏰" + Emote + LocationDesc,
            DiscordChannelType.CastleEvents => "🏰" + Emote + LocationEvents,

            DiscordChannelType.VillageParent => LocationType.Village.Localize(),
            DiscordChannelType.VillageDesc => "🎑" + Emote + LocationDesc,
            DiscordChannelType.VillageEvents => "🎑" + Emote + LocationEvents,

            DiscordChannelType.AfkParent => "zzz",
            DiscordChannelType.Afk => "Афк, жду подарки",

            DiscordChannelType.AdministrationParent => "скрытый раздел",
            DiscordChannelType.Administration => Emote + "администраторы",
            DiscordChannelType.Moderation => Emote + "модераторы",
            DiscordChannelType.EventManager => Emote + "организаторы",
            DiscordChannelType.Staff => Emote + "стафф",
            DiscordChannelType.Meeting => "Собрание",

            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

        public static DiscordChannelCategoryType Category(this DiscordChannelType channel) => channel switch
        {
            DiscordChannelType.Chat => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Commands => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.GetRoles => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.SearchParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.SearchGenshinImpact => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchLeagueOfLegends => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchTeamfightTactics => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchValorant => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchApexLegends => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchDota => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchMinecraft => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchAmongUs => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchOsu => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchRust => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchCsGo => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchHotS => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchNewWorld => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.SearchMobileGaming => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.EventParent => DiscordChannelCategoryType.CategoryChannel,
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
            DiscordChannelType.Staff => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Meeting => DiscordChannelCategoryType.VoiceChannel,

            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

        public static DiscordChannelType Parent(this DiscordChannelType channel) => channel switch
        {
            DiscordChannelType.SearchGenshinImpact => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchLeagueOfLegends => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchTeamfightTactics => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchValorant => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchApexLegends => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchDota => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchMinecraft => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchOsu => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchAmongUs => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchRust => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchHotS => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchNewWorld => DiscordChannelType.SearchParent,
            DiscordChannelType.SearchMobileGaming => DiscordChannelType.SearchParent,

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
            DiscordChannelType.Staff => DiscordChannelType.AdministrationParent,
            DiscordChannelType.Meeting => DiscordChannelType.AdministrationParent,

            _ => channel
        };
    }
}
