using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Food.Commands
{
    public record AddFoodToUserCommand(long UserId, Guid FoodId, uint Amount) : IRequest;

    public class AddFoodToUserHandler : IRequestHandler<AddFoodToUserCommand>
    {
        private readonly ILogger<AddFoodToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddFoodToUserHandler(
            DbContextOptions options,
            ILogger<AddFoodToUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddFoodToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserFoods
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.FoodId == request.FoodId);

            if (entity is null)
            {
                await _db.CreateEntity(new UserFood
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    FoodId = request.FoodId,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user food entity for user {UserId} with food {FoodId} and amount {Amount}",
                    request.UserId, request.FoodId, request.Amount);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} food {FoodId} amount {Amount}",
                    request.UserId, request.FoodId, request.Amount);
            }

            return Unit.Value;
        }
    }
}
