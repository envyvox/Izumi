using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Gathering.Commands
{
    public record AddGatheringToUserCommand(long UserId, Guid GatheringId, uint Amount) : IRequest;

    public class AddGatheringToUserHandler : IRequestHandler<AddGatheringToUserCommand>
    {
        private readonly ILogger<AddGatheringToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddGatheringToUserHandler(
            DbContextOptions options,
            ILogger<AddGatheringToUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddGatheringToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserGatherings
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.GatheringId == request.GatheringId);

            if (entity is null)
            {
                await _db.CreateEntity(new UserGathering
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    GatheringId = request.GatheringId,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user gathering entity for user {UserId} with gathering {GatheringId} and amount {Amount}",
                    request.UserId, request.GatheringId, request.Amount);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} gathering {GatheringId} amount {Amount}",
                    request.UserId, request.GatheringId, request.Amount);
            }

            return Unit.Value;
        }
    }
}
