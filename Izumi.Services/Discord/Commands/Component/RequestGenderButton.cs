using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Izumi.Services.Discord.Commands.Component
{
    public record RequestGenderButton(SocketMessageComponent Component) : IRequest;

    public class RequestGenderButtonHandler : IRequestHandler<RequestGenderButton>
    {
        private readonly IMediator _mediator;

        public RequestGenderButtonHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RequestGenderButton request, CancellationToken ct)
        {
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Component.User.Id));

            var embed = new EmbedBuilder()
                .WithColor(new Color(uint.Parse(user.CommandColor, NumberStyles.HexNumber)));

            if (user.Gender is not GenderType.None)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                    $"твой пол уже подтвержден на {emotes.GetEmote(user.Gender.EmoteName())} {user.Gender.Localize()}.");
            }
            else
            {
                var roles = DiscordRepository.Roles;

                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                    "наши курьеры доставили твою заявку на подтверждение пола. " +
                    $"Скоро с тобой свяжется <@&{roles[DiscordRoleType.Moderator].Id}> и пригласит тебя в **голосовой канал**.");

                var notify = new EmbedBuilder()
                    .WithDefaultColor()
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                        $"просит подтвердить ему {emotes.GetEmote(GenderType.None.EmoteName())} пол." +
                        "\nПригласите его/ее в **голосовой канал** для быстрой беседы.");

                var component = new ComponentBuilder()
                    .WithButton("Подтвердить мужской пол", $"update-gender-male_{user.Id}",
                        emote: Parse(emotes.GetEmote(GenderType.Male.EmoteName())))
                    .WithButton("Подтвердить женский пол", $"update-gender-female_{user.Id}",
                        emote: Parse(emotes.GetEmote(GenderType.Female.EmoteName())));

                await _mediator.Send(new SendEmbedToChannelCommand(
                    DiscordChannelType.Moderation, notify, component, $"<@&{roles[DiscordRoleType.Moderator].Id}>"));
            }

            await request.Component.FollowupAsync(embed: embed.Build(), ephemeral: true);

            return Unit.Value;
        }
    }
}