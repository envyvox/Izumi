using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Tutorial.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Tutorial.Queries
{
    public record GetUserTutorialStepQuery(long UserId) : IRequest<TutorialStepType?>;

    public class GetUserTutorialStepHandler : IRequestHandler<GetUserTutorialStepQuery, TutorialStepType?>
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public GetUserTutorialStepHandler(
            DbContextOptions options,
            IMediator mediator)
        {
            _mediator = mediator;
            _db = new AppDbContext(options);
        }

        public async Task<TutorialStepType?> Handle(GetUserTutorialStepQuery request, CancellationToken ct)
        {
            var entity = await _db.UserTutorials
                .SingleOrDefaultAsync(x => x.UserId == request.UserId);

            return entity?.Step;
        }
    }
}
