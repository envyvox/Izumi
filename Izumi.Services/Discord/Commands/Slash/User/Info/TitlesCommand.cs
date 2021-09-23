using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Title.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Info
{
    public record TitlesCommand(SocketSlashCommand Command) : IRequest;

    public class TitlesHandler : IRequestHandler<TitlesCommand>
    {
        private readonly IMediator _mediator;

        public TitlesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(TitlesCommand request, CancellationToken cancellationToken)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userTitles = await _mediator.Send(new GetUserTitlesQuery(user.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Коллекция титулов")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображаются твои титулы:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши `/титул [номер титула]` чтобы изменить текущий титул." +
                    $"\n{StringExtensions.EmptyChar}");

            var counter = 0;
            foreach (var title in userTitles.Take(16))
            {
                counter++;

                embed.AddField(
                    $"{emotes.GetEmote("List")} `{title.GetHashCode()}` {emotes.GetEmote(title.EmoteName())} {title.Localize()}",
                    StringExtensions.EmptyChar, true);

                if (counter == 2)
                {
                    counter = 0;

                    embed.AddEmptyField(true);
                }

                if (userTitles.Count > 16) embed.WithFooter("Тут отображаются только первые 16 твоих титулов.");
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}
