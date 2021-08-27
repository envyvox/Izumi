using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seafood.Commands
{
    public record AddSeafoodToUserCommand(long UserId, Guid SeafoodId, uint Amount) : IRequest;

    public class AddSeafoodToUserHandler : IRequestHandler<AddSeafoodToUserCommand>
    {
        private readonly AppDbContext _db;

        public AddSeafoodToUserHandler(DbContextOptions options)
        {
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
