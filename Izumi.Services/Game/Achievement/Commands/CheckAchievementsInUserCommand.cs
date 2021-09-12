using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using MediatR;

namespace Izumi.Services.Game.Achievement.Commands
{
    public record CheckAchievementsInUserCommand(long UserId, IEnumerable<AchievementType> Types) : IRequest;

    public class CheckAchievementsInUserHandler : IRequestHandler<CheckAchievementsInUserCommand>
    {
        private readonly IMediator _mediator;

        public CheckAchievementsInUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckAchievementsInUserCommand request, CancellationToken ct)
        {
            foreach (var type in request.Types)
                await _mediator.Send(new CheckAchievementInUserCommand(request.UserId, type));

            return Unit.Value;
        }
    }
}
