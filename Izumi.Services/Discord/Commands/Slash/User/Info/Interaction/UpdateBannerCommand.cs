using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Game.Banner.Commands;
using Izumi.Services.Game.Banner.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Info.Interaction
{
    public record UpdateBannerCommand(SocketSlashCommand Command) : IRequest;

    public class UpdateBannerHandler : IRequestHandler<UpdateBannerCommand>
    {
        private readonly IMediator _mediator;

        public UpdateBannerHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateBannerCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var banner = await _mediator.Send(new GetBannerByIncIdQuery(
                (long) request.Command.Data.Options.First().Value));
            var userActiveBanner = await _mediator.Send(new GetUserActiveBannerQuery(user.Id));

            await _mediator.Send(new ActivateUserBannerCommand(user.Id, banner.Id));
            await _mediator.Send(new DeactivateUserBannerCommand(user.Id, userActiveBanner.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Обновление баннера")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "твой баннер успешно обновлен.");

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}
