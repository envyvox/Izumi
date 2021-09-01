using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seed.Commands
{
    public record AddSeedToUserCommand(long UserId, Guid SeedId, uint Amount) : IRequest;

    public class AddSeedToUserHandler : IRequestHandler<AddSeedToUserCommand>
    {
        private readonly AppDbContext _db;

        public AddSeedToUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddSeedToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserSeeds
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.SeedId == request.SeedId);

            if (entity is null)
            {
                await _db.CreateEntity(new UserSeed
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    SeedId = request.SeedId,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);
            }

            return Unit.Value;
        }
    }
}
