using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Drink.Commands
{
    public record AddDrinkToUserCommand(long UserId, Guid DrinkId, uint Amount) : IRequest;

    public class AddDrinkToUserHandler : IRequestHandler<AddDrinkToUserCommand>
    {
        private readonly AppDbContext _db;

        public AddDrinkToUserHandler(DbContextOptions options)
        {
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
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);
            }

            return Unit.Value;
        }
    }
}
