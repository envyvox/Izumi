using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Food.Commands
{
    public record RemoveFoodFromUserCommand(long UserId, Guid FoodId, uint Amount) : IRequest;

    public class RemoveFoodFromUserHandler : IRequestHandler<RemoveFoodFromUserCommand>
    {
        private readonly AppDbContext _db;

        public RemoveFoodFromUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveFoodFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserFoods
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.FoodId == request.FoodId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have entity with food {request.FoodId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
