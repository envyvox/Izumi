using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Drink.Commands
{
    public record RemoveDrinkFromUserCommand(long UserId, Guid DrinkId, uint Amount) : IRequest;

    public class RemoveDrinkFromUserHandler : IRequestHandler<RemoveDrinkFromUserCommand>
    {
        private readonly ILogger<RemoveDrinkFromUserHandler> _logger;
        private readonly AppDbContext _db;

        public RemoveDrinkFromUserHandler(
            DbContextOptions options,
            ILogger<RemoveDrinkFromUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveDrinkFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserDrinks
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.DrinkId == request.DrinkId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have entity with drink {request.DrinkId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed from user {UserId} drink {DrinkId} amount {Amount}",
                request.UserId, request.DrinkId, request.Amount);

            return Unit.Value;
        }
    }
}
