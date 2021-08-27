using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seafood.Commands
{
    public record RemoveSeafoodFromUserCommand(long UserId, Guid SeafoodId, uint Amount) : IRequest;

    public class RemoveSeafoodFromUserHandler : IRequestHandler<RemoveSeafoodFromUserCommand>
    {
        private readonly AppDbContext _db;

        public RemoveSeafoodFromUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveSeafoodFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserSeafoods
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.SeafoodId == request.SeafoodId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have entity with seafood {request.SeafoodId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
