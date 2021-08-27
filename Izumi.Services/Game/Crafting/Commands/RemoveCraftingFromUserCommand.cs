using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crafting.Commands
{
    public record RemoveCraftingFromUserCommand(long UserId, Guid CraftingId, uint Amount) : IRequest;

    public class RemoveCraftingFromUserHandler : IRequestHandler<RemoveCraftingFromUserCommand>
    {
        private readonly AppDbContext _db;

        public RemoveCraftingFromUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveCraftingFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserCraftings
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.CraftingId == request.CraftingId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have entity with crafting {request.CraftingId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
