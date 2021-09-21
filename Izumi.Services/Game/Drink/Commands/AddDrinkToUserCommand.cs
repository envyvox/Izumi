using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Drink.Commands
{
    public record AddDrinkToUserCommand(long UserId, Guid DrinkId, uint Amount) : IRequest;

    public class AddDrinkToUserHandler : IRequestHandler<AddDrinkToUserCommand>
    {
        private readonly ILogger<AddDrinkToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddDrinkToUserHandler(
            DbContextOptions options,
            ILogger<AddDrinkToUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddDrinkToUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.UserDrinks
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.DrinkId == request.DrinkId);

            if (entity is null)
            {
                await _db.CreateEntity(new UserDrink
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    DrinkId = request.DrinkId,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Create user drink entity for user {UserId} with drink {DrinkId} and amount {Amount}",
                    request.UserId, request.DrinkId, request.Amount);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} drink {DrinkId} amount {Amount}",
                    request.UserId, request.DrinkId, request.Amount);
            }

            return Unit.Value;
        }
    }
}
