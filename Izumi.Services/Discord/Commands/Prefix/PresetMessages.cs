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

            var embed = new EmbedBuilder()
                .WithDefaultColor()
                .WithAuthor("Игровые роли")
                .WithDescription(
                    "Ты можешь получить игровые роли, которые открывают доступ к текстовым каналам где можно " +
                    "как **найти людей** для совместной игры, так и просто пообщаться на игровую тематику. " +
                    "Для этого **выбери роли** из списка под этим сообщением.")
                .WithFooter("Игровые роли можно при необходимости снять, убрав их из списка.")
                .WithImageUrl("https://cdn.discordapp.com/attachments/842067362139209778/898169768266309662/unknown.png");

            var menu = new ComponentBuilder()
                .WithSelectMenu(new SelectMenuBuilder()
                    .WithPlaceholder("Выберите игровые роли")
                    .WithCustomId("select-game-roles")
                    .WithMinValues(0)
                    .WithMaxValues(14)
                    .AddOption("Genshin Impact", "GenshinImpact", emote: Parse(emotes.GetEmote("GenshinImpact")))
                    .AddOption("League of Legends", "LeagueOfLegends", emote: Parse(emotes.GetEmote("LeagueOfLegends")))
                    .AddOption("Teamfight Tactics", "TeamfightTactics", emote: Parse(emotes.GetEmote("TeamfightTactics")))
                    .AddOption("Valorant", "Valorant", emote: Parse(emotes.GetEmote("Valorant")))
                    .AddOption("Apex Legends", "ApexLegends", emote: Parse(emotes.GetEmote("ApexLegends")))
                    .AddOption("Lost Ark", "LostArk", emote: Parse(emotes.GetEmote("LostArk")))
                    .AddOption("Dota 2", "Dota", emote: Parse(emotes.GetEmote("Dota")))
                    .AddOption("Among Us", "AmongUs", emote: Parse(emotes.GetEmote("AmongUs")))
                    .AddOption("Osu", "Osu", emote: Parse(emotes.GetEmote("Osu")))
                    .AddOption("Rust", "Rust", emote: Parse(emotes.GetEmote("Rust")))
                    .AddOption("CS:GO", "CSGO", emote: Parse(emotes.GetEmote("CSGO")))
                    .AddOption("HotS", "HotS", emote: Parse(emotes.GetEmote("HotS")))
                    .AddOption("Wild Rift", "WildRift", emote: Parse(emotes.GetEmote("WildRift")))
                    .AddOption("Mobile Legends", "MobileLegends", emote: Parse(emotes.GetEmote("MobileLegends")))
                    .AddOption("New World", "NewWorld", emote: Parse(emotes.GetEmote("NewWorld"))));

            await Context.Channel.SendMessageAsync(embed: embed.Build(), component: menu.Build());
        }

        [Command("event-role")]
        public async Task SendEventRoleMessageTask()
        {
            var channels = await _mediator.Send(new GetChannelsQuery());
            var roles = await _mediator.Send(new GetRolesQuery());

            var embed = new EmbedBuilder()
                .WithDefaultColor()
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
                .WithDefaultColor()
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
