using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Commands
{
    public record AddAlcoholToUserCommand(long UserId, Guid AlcoholId, uint Amount) : IRequest;

    public class AddAlcoholToUserHandler : IRequestHandler<AddAlcoholToUserCommand>
    {
        private readonly AppDbContext _db;

        public AddAlcoholToUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddAlcoholToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserAlcohols
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.AlcoholId == request.AlcoholId);

            if (entity is null)
            {
                await _db.CreateEntity(new UserAlcohol
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    AlcoholId = request.AlcoholId,
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
