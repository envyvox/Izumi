using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Info
{
    public record AchievementsCommand(SocketSlashCommand Command) : IRequest;

    public class AchievementsHandler : IRequestHandler<AchievementsCommand>
    {
        private readonly IMediator _mediator;

        public AchievementsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AchievementsCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var category = (AchievementCategoryType) (long) request.Command.Data.Options.First().Value;

            var embed = new EmbedBuilder()
                .WithAuthor("Достижения")
                .WithDescription(
                    $".")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Achievements)));

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}
