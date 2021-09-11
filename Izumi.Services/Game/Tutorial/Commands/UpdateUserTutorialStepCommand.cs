using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Tutorial.Commands
{
    public record UpdateUserTutorialStepCommand(long UserId, TutorialStepType Step) : IRequest;

    public class UpdateUserTutorialStepHandler : IRequestHandler<UpdateUserTutorialStepCommand>
    {
        private readonly AppDbContext _db;

        public UpdateUserTutorialStepHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateUserTutorialStepCommand request, CancellationToken ct)
        {
            var entity = await _db.UserTutorials
                .SingleOrDefaultAsync(x => x.UserId == request.UserId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt started tutorial");
            }

            entity.Step = request.Step;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
