using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using MediatR;
using static Discord.Emote;

namespace Izumi.Services.Discord.Commands.Prefix
{
    [RequireOwner]
    [Group("preset")]
    public class PresetMessages : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public PresetMessages(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("game-roles")]
        public async Task SendGameRolesMessageTask()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var channels = await _mediator.Send(new GetChannelsQuery());

            var embed = new EmbedBuilder()
                .WithColor(new Color(uint.Parse("202225", NumberStyles.HexNumber)))
                .WithAuthor("Игровые роли")
                .WithDescription(
                    $"Ты можешь получить игровые роли, которые можно **упоминать** в <#{channels[DiscordChannelType.Search].Id}> " +
                    "чтобы упростить процесс **поиска людей** для совместной игры, для этого нажми на **кнопку** под этим сообщением.");

            var buttons = new ComponentBuilder()
                .WithButton("Genshin Impact", "toggle-role-GenshinImpact", emote: Parse(emotes.GetEmote("GenshinImpact")))
                .WithButton("League of Legends", "toggle-role-LeagueOfLegends", emote: Parse(emotes.GetEmote("LeagueOfLegends")))
                .WithButton("Teamfight Tactics", "toggle-role-TeamfightTactics", emote: Parse(emotes.GetEmote("TeamfightTactics")))
                .WithButton("Valorant", "toggle-role-Valorant", emote: Parse(emotes.GetEmote("Valorant")))
                .WithButton("Apex Legends", "toggle-role-ApexLegends", emote: Parse(emotes.GetEmote("ApexLegends")))
                .WithButton("Lost Ark", "toggle-role-LostArk", emote: Parse(emotes.GetEmote("LostArk")))
                .WithButton("Dota 2", "toggle-role-Dota", emote: Parse(emotes.GetEmote("Dota")))
                .WithButton("Among Us", "toggle-role-AmongUs", emote: Parse(emotes.GetEmote("AmongUs")))
                .WithButton("Osu", "toggle-role-Osu", emote: Parse(emotes.GetEmote("Osu")))
                .WithButton("Rust", "toggle-role-Rust", emote: Parse(emotes.GetEmote("Rust")))
                .WithButton("CS:GO", "toggle-role-CSGO", emote: Parse(emotes.GetEmote("CSGO")))
                .WithButton("HotS", "toggle-role-HotS", emote: Parse(emotes.GetEmote("HotS")))
                .WithButton("Wild Rift", "toggle-role-WildRift", emote: Parse(emotes.GetEmote("WildRift")))
                .WithButton("Mobile Legends", "toggle-role-MobileLegends", emote: Parse(emotes.GetEmote("MobileLegends")));

            await Context.Channel.SendMessageAsync("", false, embed.Build(), component: buttons.Build());
        }

        [Command("event-role")]
        public async Task SendEventRoleMessageTask()
        {
            var channels = await _mediator.Send(new GetChannelsQuery());
            var roles = await _mediator.Send(new GetRolesQuery());

            var embed = new EmbedBuilder()
                .WithColor(new Color(uint.Parse("202225", NumberStyles.HexNumber)))
                .WithAuthor("Роль мероприятия")
                .WithDescription(
                    $"Ты можешь получить роль <@&{roles[DiscordRoleType.DiscordEvent].Id}>, " +
                    $"которая будет **упоминаться** в <#{channels[DiscordChannelType.EventNotification].Id}> " +
                    "для оповещения о предстоящих **мероприятиях**, для этого нажми на **кнопку** под этим сообщением.")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.GetEventRole)));

            var buttons = new ComponentBuilder()
                .WithButton("Мероприятия", "toggle-role-DiscordEvent", emote: new Emoji("🥳"));

            await Context.Channel.SendMessageAsync("", false, embed.Build(), component: buttons.Build());
        }

        [Command("how-desc-work")]
        public async Task SendHowCommunityDescWorkMessageTask()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var channels = await _mediator.Send(new GetChannelsQuery());
            var roles = await _mediator.Send(new GetRolesQuery());

            var embed = new EmbedBuilder()
                .WithColor(new Color(uint.Parse("202225", NumberStyles.HexNumber)))
                .WithAuthor("Доска сообщества")
                .WithDescription(
                    "Ты можешь делиться с нами своими любимыми изображениями в каналах доски сообщества." +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши в канал <#{channels[DiscordChannelType.Commands].Id}> " +
                    "`/доска-сообщества` чтобы посмотреть информацию о своем участии." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Каналы доски",
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Photos].Id}> - Красивые ~~котики~~ фотографии.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Screenshots].Id}> - Твои яркие моменты.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Memes].Id}> - Говорящее само за себя название канала.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Arts].Id}> - Красивые рисунки.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Erotic].Id}> - Изображения, носящие эротический характер.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Nsfw].Id}> - Изображения 18+." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Получение роли",
                    $"Пользователи, публикации который набирают суммарно {emotes.GetEmote("Like")} 500 лайков " +
                    $"получают роль <@&{roles[DiscordRoleType.ContentProvider].Id}> на 30 дней." +
                    $"\n\n{emotes.GetEmote("Arrow")} Если пользователь получит еще {emotes.GetEmote("Like")} 500 лайков " +
                    "уже имя роль, то ее длительность увеличится на 30 дней." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Модерация",
                    $"Публикации набирающие {emotes.GetEmote("Dislike")} 5 дизлайков будут автоматически удалены." +
                    $"\n\n{emotes.GetEmote("Arrow")} Публикации нарушающие правила сервера или правила отдельных " +
                    "каналов будут удалены модерацией сервера без предупреждения.");

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
