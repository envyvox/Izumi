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
using MediatR;

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
            var roles = await _mediator.Send(new GetRolesQuery());

            var embed = new EmbedBuilder()
                .WithColor(new Color(uint.Parse("202225", NumberStyles.HexNumber)))
                .WithAuthor("Игровые роли")
                .WithDescription(
                    $"Ты можешь получить игровые роли, которые можно **упоминать** в <#{channels[DiscordChannelType.Search].Id}> " +
                    "чтобы упростить процесс **поиска людей** для совместной игры, для этого нажми на **реакцию** под этим сообщением.")
                .AddField("Доступные для получения роли",
                    $"{emotes.GetEmote("GenshinImpact")} <@&{roles[DiscordRoleType.GenshinImpact].Id}>\n" +
                    $"{emotes.GetEmote("LeagueOfLegends")} <@&{roles[DiscordRoleType.LeagueOfLegends].Id}>\n" +
                    $"{emotes.GetEmote("TeamfightTactics")} <@&{roles[DiscordRoleType.TeamfightTactics].Id}>\n" +
                    $"{emotes.GetEmote("Valorant")} <@&{roles[DiscordRoleType.Valorant].Id}>\n" +
                    $"{emotes.GetEmote("ApexLegends")} <@&{roles[DiscordRoleType.ApexLegends].Id}>\n" +
                    $"{emotes.GetEmote("LostArk")} <@&{roles[DiscordRoleType.LostArk].Id}>\n" +
                    $"{emotes.GetEmote("Dota")} <@&{roles[DiscordRoleType.Dota].Id}>\n" +
                    $"{emotes.GetEmote("AmongUs")} <@&{roles[DiscordRoleType.AmongUs].Id}>\n" +
                    $"{emotes.GetEmote("Osu")} <@&{roles[DiscordRoleType.Osu].Id}>\n" +
                    $"{emotes.GetEmote("Rust")} <@&{roles[DiscordRoleType.Rust].Id}>\n" +
                    $"{emotes.GetEmote("CSGO")} <@&{roles[DiscordRoleType.CsGo].Id}>\n" +
                    $"{emotes.GetEmote("HotS")} <@&{roles[DiscordRoleType.HotS].Id}>\n" +
                    $"{emotes.GetEmote("WildRift")} <@&{roles[DiscordRoleType.WildRift].Id}>\n" +
                    $"{emotes.GetEmote("MobileLegends")} <@&{roles[DiscordRoleType.MobileLegends].Id}>\n")
                .WithFooter(
                    "При нажатии на реакцию, она будет снята и ты получишь соответствующую роль. " +
                    "При необходимости роль можно снять, нажав на реакцию повторно.");

            var message = await Context.Channel.SendMessageAsync("", false, embed.Build());

            await message.AddReactionsAsync(new IEmote[]
            {
                global::Discord.Emote.Parse(emotes.GetEmote("GenshinImpact")),
                global::Discord.Emote.Parse(emotes.GetEmote("LeagueOfLegends")),
                global::Discord.Emote.Parse(emotes.GetEmote("TeamfightTactics")),
                global::Discord.Emote.Parse(emotes.GetEmote("Valorant")),
                global::Discord.Emote.Parse(emotes.GetEmote("ApexLegends")),
                global::Discord.Emote.Parse(emotes.GetEmote("LostArk")),
                global::Discord.Emote.Parse(emotes.GetEmote("Dota")),
                global::Discord.Emote.Parse(emotes.GetEmote("AmongUs")),
                global::Discord.Emote.Parse(emotes.GetEmote("Osu")),
                global::Discord.Emote.Parse(emotes.GetEmote("Rust")),
                global::Discord.Emote.Parse(emotes.GetEmote("CSGO")),
                global::Discord.Emote.Parse(emotes.GetEmote("HotS")),
                global::Discord.Emote.Parse(emotes.GetEmote("WildRift")),
                global::Discord.Emote.Parse(emotes.GetEmote("MobileLegends"))
            });
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
                    "для оповещения о предстоящих **мероприятиях**, для этого нажми на **реакцию** под этим сообщением.")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.GetEventRole)))
                .WithFooter(
                    "При нажатии на реакцию, она будет снята и ты получишь соответствующую роль. " +
                    "При необходимости роль можно снять, нажав на реакцию повторно.");

            var message = await Context.Channel.SendMessageAsync("", false, embed.Build());

            await message.AddReactionAsync(new Emoji("🥳"));
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
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши `/доска-сообщества` чтобы посмотреть информацию о своем участии." +
                    $"\n{emotes.GetEmote("Blank")}")
                .AddField("Каналы доски",
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Photos].Id}> - Красивые ~~котики~~ фотографии.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Screenshots].Id}> - Твои яркие моменты.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Memes].Id}> - Говорящее само за себя название канала.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Arts].Id}> - Красивые рисунки.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Erotic].Id}> - Изображения, носящие эротический характер.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Nsfw].Id}> - Изображения 18+." +
                    $"\n{emotes.GetEmote("Blank")}")
                .AddField("Получение роли",
                    $"Пользователи, публикации который набирают суммарно {emotes.GetEmote("Like")} 500 лайков " +
                    $"получают роль <@&{roles[DiscordRoleType.ContentProvider].Id}> на 30 дней." +
                    $"\n\n{emotes.GetEmote("Arrow")} Если пользователь получит еще {emotes.GetEmote("Like")} 500 лайков " +
                    "уже имя роль, то ее длительность увеличится на 30 дней." +
                    $"\n{emotes.GetEmote("Blank")}")
                .AddField("Модерация",
                    $"Публикации набирающие {emotes.GetEmote("Dislike")} 5 дизлайков будут автоматически удалены." +
                    $"\n\n{emotes.GetEmote("Arrow")} Публикации нарушающие правила сервера или правила отдельных " +
                    "каналов будут удалены модерацией сервера без предупреждения.");

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
