using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Cooldown.Commands
{
    public record CreateUserCooldownCommand(long UserId, CooldownType Type, TimeSpan Duration) : IRequest;

    public class CreateUserCooldownHandler : IRequestHandler<CreateUserCooldownCommand>
    {
        private readonly AppDbContext _db;

        public CreateUserCooldownHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateUserCooldownCommand request, CancellationToken ct)
        {
            var entity = await _db.UserCooldowns
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (entity is null)
            {
                await _db.CreateEntity(new UserCooldown
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Type = request.Type,
                    Expiration = DateTimeOffset.UtcNow.Add(request.Duration),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });
            }
            else
            {
                entity.Expiration = DateTimeOffset.UtcNow.Add(request.Duration);
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);
            }

            return Unit.Value;
        }
    }
}
