using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Field.Commands
{
    public record ResetUserFieldCommand(long UserId, uint Number) : IRequest;

    public class ResetUserFieldHandler : IRequestHandler<ResetUserFieldCommand>
    {
        private readonly ILogger<ResetUserFieldHandler> _logger;
        private readonly AppDbContext _db;

        public ResetUserFieldHandler(
            DbContextOptions options,
            ILogger<ResetUserFieldHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(ResetUserFieldCommand request, CancellationToken ct)
        {
            var entity = await _db.UserFields
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Number == request.Number);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have field {request.Number}");
            }

            entity.State = FieldStateType.Empty;
            entity.SeedId = null;
            entity.Progress = 0;
            entity.InReGrowth = false;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Reseted user {UserId} field {Number} to default values",
                request.UserId, request.Number);

            return Unit.Value;
        }
    }
}
