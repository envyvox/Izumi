using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.Discord;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Role.Commands
{
    public record AddRoleToUserCommand(long UserId, long RoleId, TimeSpan Duration) : IRequest;

    public class AddRoleToUserHandler : IRequestHandler<AddRoleToUserCommand>
    {
        private readonly ILogger<AddRoleToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddRoleToUserHandler(
            DbContextOptions options,
            ILogger<AddRoleToUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddRoleToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserRoles
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.RoleId == request.RoleId);

            if (entity is null)
            {
                await _db.CreateEntity(new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    RoleId = request.RoleId,
                    Expiration = DateTimeOffset.UtcNow.Add(request.Duration),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user role entity, UserId: {UserId}, RoleId: {RoleId}, Expiration: {Expiration}",
                    request.UserId, request.RoleId, DateTimeOffset.UtcNow.Add(request.Duration));
            }
            else
            {
                entity.Expiration = entity.Expiration.Add(request.Duration);
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Updated user role entity, UserId: {UserId}, RoleId: {RoleId}, new Expiration: {Expiration}",
                    request.UserId, request.RoleId, entity.Expiration);
            }

            return Unit.Value;
        }
    }
}
