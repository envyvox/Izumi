using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Cooking
{
    public record CookingStartCommand(SocketSlashCommand Command) : IRequest;

    public class CookingStartHandler : IRequestHandler<CookingStartCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CookingStartHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(CookingStartCommand request, CancellationToken ct)
        {
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            // todo move this check into background job and add condition
            await _mediator.Send(new CheckUserTutorialStepCommand(user.Id, TutorialStepType.CookFriedEgg));

            return await _mediator.Send(new RespondEmbedCommand(request.Command, new EmbedBuilder()
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "данный функционал находится в разработке. " +
                    "Следи за каналом <#750624435702333460> чтобы быть в курсе всех обновлений.")));
        }
    }
}
