using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Fish.Commands
{
    public record RemoveFishFromUserCommand(long UserId, Guid FishId, uint Amount) : IRequest;

    public class RemoveFishFromUserHandler : IRequestHandler<RemoveFishFromUserCommand>
    {
        private readonly AppDbContext _db;

        public RemoveFishFromUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveFishFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserFishes
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.FishId == request.FishId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have entity with fish {request.FishId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
