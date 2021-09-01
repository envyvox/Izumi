using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seed.Commands
{
    public record RemoveSeedFromUserCommand(long UserId, Guid SeedId, uint Amount) : IRequest;

    public class RemoveSeedFromUserHandler : IRequestHandler<RemoveSeedFromUserCommand>
    {
        private readonly AppDbContext _db;

        public RemoveSeedFromUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveSeedFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserSeeds
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.SeedId == request.SeedId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have seed {request.SeedId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
