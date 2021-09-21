using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Gathering.Commands
{
    public record RemoveGatheringFromUserCommand(long UserId, Guid GatheringId, uint Amount) : IRequest;

    public class RemoveGatheringFromUserHandler : IRequestHandler<RemoveGatheringFromUserCommand>
    {
        private readonly ILogger<RemoveGatheringFromUserHandler> _logger;
        private readonly AppDbContext _db;

        public RemoveGatheringFromUserHandler(
            DbContextOptions options,
            ILogger<RemoveGatheringFromUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveGatheringFromUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.UserGatherings
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.GatheringId == request.GatheringId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have entity with gathering {request.GatheringId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed from user {UserId} gathering {GatheringId} amount {Amount}",
                request.UserId, request.GatheringId, request.Amount);

            return Unit.Value;
        }
    }
}
