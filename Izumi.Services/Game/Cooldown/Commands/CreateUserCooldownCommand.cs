using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Cooldown.Commands
{
    public record CreateUserCooldownCommand(long UserId, CooldownType Type, TimeSpan Duration) : IRequest;

    public class CreateUserCooldownHandler : IRequestHandler<CreateUserCooldownCommand>
    {
        private readonly ILogger<CreateUserCooldownHandler> _logger;
        private readonly AppDbContext _db;

        public CreateUserCooldownHandler(
            DbContextOptions options,
            ILogger<CreateUserCooldownHandler> logger)
        {
            _logger = logger;
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

                _logger.LogInformation(
                    "Created user cooldown for user {UserId} with type {Type} and expiration {Expiration}",
                    request.UserId, request.Type.ToString(), DateTimeOffset.UtcNow.Add(request.Duration));
            }
            else
            {
                entity.Expiration = DateTimeOffset.UtcNow.Add(request.Duration);
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Updated user cooldown for user {UserId} with type {Type} and new expiration {Expiration}",
                    request.UserId, request.Type.ToString(), DateTimeOffset.UtcNow.Add(request.Duration));
            }

            return Unit.Value;
        }
    }
}
