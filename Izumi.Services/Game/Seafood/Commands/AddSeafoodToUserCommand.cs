using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Seafood.Commands
{
    public record AddSeafoodToUserCommand(long UserId, Guid SeafoodId, uint Amount) : IRequest;

    public class AddSeafoodToUserHandler : IRequestHandler<AddSeafoodToUserCommand>
    {
        private readonly ILogger<AddSeafoodToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddSeafoodToUserHandler(
            DbContextOptions options,
            ILogger<AddSeafoodToUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddSeafoodToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserSeafoods
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.SeafoodId == request.SeafoodId);

            if (entity is null)
            {
                await _db.CreateEntity(new UserSeafood
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    SeafoodId = request.SeafoodId,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user seafood entity for user {UserId} with seafood {SeafoodId} and amount {Amount}",
                    request.UserId, request.SeafoodId, request.Amount);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} seafood {SeafoodId} amount {Amount}",
                    request.UserId, request.SeafoodId, request.Amount);
            }

            return Unit.Value;
        }
    }
}
