using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Game.Title.Queries;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Info.Interaction
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

            if (!hasTitle)
            {
                await Task.FromException(new Exception(
                    $"у тебя нет титула {emotes.GetEmote(title.EmoteName())} {title.Localize()}."));
            }
            else
            {
                await _mediator.Send(new UpdateUserTitleCommand(user.Id, title));

                var embed = new EmbedBuilder()
                    .WithAuthor("Обновление титула")
                    .WithDescription(
                        $"{emotes.GetEmote(title.EmoteName())} {title.Localize()} {request.Command.User.Mention}, " +
                        "твой титул успешно обновлен.");

                await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
            }

            return Unit.Value;
        }
    }
}
