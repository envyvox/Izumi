using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Settings
{
    public record SettingsGenderCommand(SocketSlashCommand Command) : IRequest;

    public class SettingsGenderHandler : IRequestHandler<SettingsGenderCommand>
    {
        private readonly IMediator _mediator;
        private readonly IDiscordClientService _discordClientService;

        public SettingsGenderHandler(
            IMediator mediator,
            IDiscordClientService discordClientService)
        {
            _mediator = mediator;
            _discordClientService = discordClientService;
        }

        public async Task<Unit> Handle(SettingsGenderCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            var embed = new EmbedBuilder();

            if (user.Gender is not GenderType.None)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"твой пол уже подтвержден на {emotes.GetEmote(user.Gender.EmoteName())} {user.Gender.Localize()}.");
            }
            else
            {
                var client = await _discordClientService.GetSocketClient();
                var roles = await _mediator.Send(new GetRolesQuery());

                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "наши курьеры доставили твою заявку на подтверждение пола. " +
                    $"Скоро с тобой свяжется <@&{roles[DiscordRoleType.Moderator].Id}> и пригласит тебя в **голосовой канал**.");

                var notify = new EmbedBuilder()
                    .WithDefaultColor()
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"просит подтвердить ему {emotes.GetEmote(GenderType.None.EmoteName())} пол." +
                        "\nПригласите его/ее в **голосовой канал** для быстрой беседы." +
                        $"\n{StringExtensions.EmptyChar}")
                    .AddField("Подтверждение пола",
                        "После беседы в голосовом канале, упомяни меня и напиши одну из команд:" +
                        $"\n{emotes.GetEmote(GenderType.Male.EmoteName())} {client.CurrentUser.Mention} update-gender {request.Command.User.Mention} 1" +
                        $"\n{emotes.GetEmote(GenderType.Female.EmoteName())} {client.CurrentUser.Mention} update-gender {request.Command.User.Mention} 2");

                await _mediator.Send(new SendEmbedToChannelCommand(
                    DiscordChannelType.Moderation, notify, $"<@&{roles[DiscordRoleType.Moderator].Id}>"));
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}
