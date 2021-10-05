using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Game.Title.Queries;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Info.Interaction
{
    public record UpdateTitleCommand(SocketSlashCommand Command) : IRequest;

    public class UpdateTitleHandler : IRequestHandler<UpdateTitleCommand>
    {
        private readonly IMediator _mediator;

        public UpdateTitleHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateTitleCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var title = (TitleType) (long) request.Command.Data.Options.First().Value;
            var hasTitle = await _mediator.Send(new CheckTitleInUserQuery(user.Id, title));

            var embed = new EmbedBuilder()
                .WithAuthor("Обновление титула");

            if (!hasTitle)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя нет титула {emotes.GetEmote(title.EmoteName())} {title.Localize()}.");
            }
            else
            {
                await _mediator.Send(new RemoveRoleFromGuildUserCommand(request.Command.User.Id, user.Title.Role()));
                await _mediator.Send(new AddRoleToGuildUserCommand(request.Command.User.Id, title.Role()));
                await _mediator.Send(new UpdateUserTitleCommand(user.Id, title));

                embed.WithDescription(
                    $"{emotes.GetEmote(title.EmoteName())} {title.Localize()} {request.Command.User.Mention}, " +
                    "твой титул успешно обновлен.");
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}
