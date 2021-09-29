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

namespace Izumi.Services.Game.Reputation.Commands
{
    public record AddReputationToUserCommand(long UserId, ReputationType Type, uint Amount) : IRequest;

    public class AddReputationToUserHandler : IRequestHandler<AddReputationToUserCommand>
    {
        private readonly ILogger<AddReputationToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddReputationToUserHandler(
            DbContextOptions options,
            ILogger<AddReputationToUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddReputationToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserReputations
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (entity is null)
            {
                await _db.CreateEntity(new UserReputation
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Type = request.Type,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user reputation entity for user {UserId} with reputation {Type} and amount {Amount}",
                    request.UserId, request.Type.ToString(), request.Amount);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} reputation {Type} amount {Amount}",
                    request.UserId, request.Type.ToString(), request.Amount);
            }

            return Unit.Value;
        }
    }
}
