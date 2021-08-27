using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Commands
{
    public record RemoveAlcoholFromUserCommand(long UserId, Guid AlcoholId, uint Amount) : IRequest;

    public class RemoveAlcoholFromUserHandler : IRequestHandler<RemoveAlcoholFromUserCommand>
    {
        private readonly AppDbContext _db;

        public RemoveAlcoholFromUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveAlcoholFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserAlcohols
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.AlcoholId == request.AlcoholId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have entity with alcohol {request.AlcoholId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
