using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Settings
{
    public record SettingsCommandColorCommand(SocketSlashCommand Command) : IRequest;

    public class SettingsCommandColorCommandHandler : IRequestHandler<SettingsCommandColorCommand>
    {
        private readonly IMediator _mediator;

        public SettingsCommandColorCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SettingsCommandColorCommand request, CancellationToken ct)
        {
            var option = ((string) request.Command.Data.Options.First().Options.First().Value).Replace("#", "");
            var color = new Color(uint.Parse(option, NumberStyles.HexNumber));

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            var embed = new EmbedBuilder();

            if (!user.IsPremium)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"этот функционал доступен только {emotes.GetEmote("Premium")} премиум пользователям.");
            }
            else
            {
                await _mediator.Send(new UpdateUserCommandColorCommand(user.Id, option));

                embed
                    .WithColor(color)
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"цвет команд успешно обновлен на `#{option}`.");
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}
