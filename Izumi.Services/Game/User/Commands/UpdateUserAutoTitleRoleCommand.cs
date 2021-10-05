using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.User.Commands
{
    public record UpdateUserAutoTitleRoleCommand(long UserId, bool AutoTitleRole) : IRequest;

    public class UpdateUserAutoTitleRoleHandler : IRequestHandler<UpdateUserAutoTitleRoleCommand>
    {
        private readonly ILogger<UpdateUserAutoTitleRoleHandler> _logger;
        private readonly AppDbContext _db;

        public UpdateUserAutoTitleRoleHandler(
            DbContextOptions options,
            ILogger<UpdateUserAutoTitleRoleHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserAutoTitleRoleCommand request, CancellationToken ct)
        {
            var entity = await _db.Users
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            entity.AutoTitleRole = request.AutoTitleRole;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Updated user {UserId} auto title role value to {Value}",
                request.UserId, request.AutoTitleRole);

            return Unit.Value;
        }
    }
}
