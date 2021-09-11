using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Tutorial.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Game.Tutorial.Commands
{
    public record CheckUserTutorialStepCommand(long UserId, TutorialStepType Step) : IRequest;

    public class CheckUserTutorialStepHandler : IRequestHandler<CheckUserTutorialStepCommand>
    {
        private readonly IMediator _mediator;

        public CheckUserTutorialStepHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckUserTutorialStepCommand request, CancellationToken ct)
        {
            var userStep = await _mediator.Send(new GetUserTutorialStepQuery(request.UserId));

            if (userStep != request.Step) return Unit.Value;

            var nextStep = (TutorialStepType) request.Step.GetHashCode() + 1;

            await _mediator.Send(new UpdateUserTutorialStepCommand(request.UserId, nextStep));

            switch (nextStep)
            {
                case TutorialStepType.CookFriedEgg:

                    // todo add fried egg recipe
                    // todo add fried egg ingredients

                    break;
                case TutorialStepType.TransitToCastle:

                    // todo add special food x30

                    break;
                case TutorialStepType.Completed:

                    var currencyAmount = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.EconomyTutorialReward));

                    await _mediator.Send(new AddCurrencyToUserCommand(request.UserId, CurrencyType.Ien,
                        currencyAmount));

                    break;
            }

            var embed = new EmbedBuilder()
                .WithAuthor(nextStep.Name())
                .WithDescription(nextStep.Description());

            return await _mediator.Send(new SendEmbedToUserCommand((ulong) request.UserId, embed));
        }
    }
}
