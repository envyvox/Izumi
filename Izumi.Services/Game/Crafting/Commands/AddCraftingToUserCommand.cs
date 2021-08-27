using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crafting.Commands
{
    public record AddCraftingToUserCommand(long UserId, Guid CraftingId, uint Amount) : IRequest;

    public class AddCraftingToUserHandler : IRequestHandler<AddCraftingToUserCommand>
    {
        private readonly AppDbContext _db;

        public AddCraftingToUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddCraftingToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserCraftings
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.CraftingId == request.CraftingId);

            if (entity is null)
            {
                await _db.CreateEntity(new UserCrafting
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    CraftingId = request.CraftingId,
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
