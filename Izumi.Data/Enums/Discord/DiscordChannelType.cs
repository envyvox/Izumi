using System;

namespace Izumi.Data.Enums.Discord
{
    public enum DiscordChannelType : byte
    {
        Welcome,
        Chat,
        Commands,
        GetRoles,
        Announcements,

        GameParent,
        GameInfo,
        GameLore,
        GameEvents,
        GameUpdates,

        SearchParent,
        SearchGenshinImpact,
        SearchLeagueOfLegends,
        SearchTeamfightTactics,
        SearchValorant,
        SearchApexLegends,
        SearchDota,
        SearchMinecraft,
        SearchOsu,
        SearchAmongUs,
        SearchRust,
        SearchCsGo,
        SearchHotS,
        SearchNewWorld,
        SearchMobileGaming,

        EventParent,

        CommunityDescParent,
        CommunityDescHowItWork,
        Photos,
        Screenshots,
        Memes,
        Arts,
        Music,
        Erotic,
        Nsfw,

        LibraryParent,
        Rules,
        Giveaways,
        Suggestions,

        TavernParent,
        TavernOne,
        TavernTwo,
        TavernMale,
        TavernFemale,

        CreateRoomParent,
        NoMic,
        CreateRoom,

        FamilyRoomParent,

        AfkParent,
        Afk,

        AdministrationParent,
        Administration,
        Moderation,
        EventManager,
        Staff,
        Meeting
    }

    public static class DiscordChannelHelper
    {
        private const string Emote = "・";
        private const string Tavern = "Таверна";

        public static string Name(this DiscordChannelType channel) => channel switch
        {
            DiscordChannelType.Welcome => Emote + "приветствие",
            DiscordChannelType.Chat => Emote + "общение",
            DiscordChannelType.Commands => Emote + "команды",
            DiscordChannelType.GetRoles => Emote + "получение-ролей",
            DiscordChannelType.Announcements => Emote + "объявления",

            DiscordChannelType.GameParent => "игровая вселенная",
            DiscordChannelType.GameInfo => Emote + "информация",
            DiscordChannelType.GameLore => Emote + "история-мира",
            DiscordChannelType.GameEvents => Emote + "игровые-события",
            DiscordChannelType.GameUpdates => Emote + "обновления",

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

            DiscordChannelType.CommunityDescParent => "доска сообщества",
            DiscordChannelType.CommunityDescHowItWork => Emote + "как-работает",
            DiscordChannelType.Photos => Emote + "фотографии",
            DiscordChannelType.Screenshots => Emote + "скриншоты",
            DiscordChannelType.Memes => Emote + "мемесы",
            DiscordChannelType.Arts => Emote + "арты",
            DiscordChannelType.Music => Emote + "музыка",
            DiscordChannelType.Erotic => Emote + "эротика",
            DiscordChannelType.Nsfw => Emote + "nsfw",

            DiscordChannelType.LibraryParent => "великая «тосёкан»",
            DiscordChannelType.Rules => Emote + "правила",
            DiscordChannelType.Giveaways => Emote + "розыгрыши",
            DiscordChannelType.Suggestions => Emote + "предложения",

            DiscordChannelType.TavernParent => "Таверны",
            DiscordChannelType.TavernOne => Tavern + " «Идзакая»",
            DiscordChannelType.TavernTwo => Tavern + " «Каябукия»",
            DiscordChannelType.TavernMale => Tavern + " «Оками»",
            DiscordChannelType.TavernFemale => Tavern + " «Китсунэ»",

            DiscordChannelType.CreateRoomParent => "пригородные лагеря",
            DiscordChannelType.NoMic => Emote + "без-микрофона",
            DiscordChannelType.CreateRoom => "Разжечь костер",

            DiscordChannelType.FamilyRoomParent => "семейные беседки",

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
            DiscordChannelType.Welcome => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Chat => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Commands => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.GetRoles => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Announcements => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.GameParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.GameInfo => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.GameLore => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.GameEvents => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.GameUpdates => DiscordChannelCategoryType.TextChannel,

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

            DiscordChannelType.CommunityDescParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.CommunityDescHowItWork => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Photos => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Screenshots => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Memes => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Arts => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Music => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Erotic => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Nsfw => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.LibraryParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.Rules => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Giveaways => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.Suggestions => DiscordChannelCategoryType.TextChannel,

            DiscordChannelType.TavernParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.TavernOne => DiscordChannelCategoryType.VoiceChannel,
            DiscordChannelType.TavernTwo => DiscordChannelCategoryType.VoiceChannel,
            DiscordChannelType.TavernMale => DiscordChannelCategoryType.VoiceChannel,
            DiscordChannelType.TavernFemale => DiscordChannelCategoryType.VoiceChannel,

            DiscordChannelType.CreateRoomParent => DiscordChannelCategoryType.CategoryChannel,
            DiscordChannelType.NoMic => DiscordChannelCategoryType.TextChannel,
            DiscordChannelType.CreateRoom => DiscordChannelCategoryType.VoiceChannel,

            DiscordChannelType.FamilyRoomParent => DiscordChannelCategoryType.CategoryChannel,

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
            DiscordChannelType.GameInfo => DiscordChannelType.GameParent,
            DiscordChannelType.GameLore => DiscordChannelType.GameParent,
            DiscordChannelType.GameEvents => DiscordChannelType.GameParent,
            DiscordChannelType.GameUpdates => DiscordChannelType.GameParent,

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

            DiscordChannelType.CommunityDescHowItWork => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Photos => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Screenshots => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Memes => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Arts => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Music => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Erotic => DiscordChannelType.CommunityDescParent,
            DiscordChannelType.Nsfw => DiscordChannelType.CommunityDescParent,

            DiscordChannelType.Rules => DiscordChannelType.LibraryParent,
            DiscordChannelType.Giveaways => DiscordChannelType.LibraryParent,
            DiscordChannelType.Suggestions => DiscordChannelType.LibraryParent,

            DiscordChannelType.TavernOne => DiscordChannelType.TavernParent,
            DiscordChannelType.TavernTwo => DiscordChannelType.TavernParent,
            DiscordChannelType.TavernMale => DiscordChannelType.TavernParent,
            DiscordChannelType.TavernFemale => DiscordChannelType.TavernParent,

            DiscordChannelType.NoMic => DiscordChannelType.CreateRoomParent,
            DiscordChannelType.CreateRoom => DiscordChannelType.CreateRoomParent,

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