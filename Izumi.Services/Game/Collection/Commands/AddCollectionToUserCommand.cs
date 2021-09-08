using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Collection.Commands
{
    public record AddCollectionToUserCommand(long UserId, CollectionType Type, Guid ItemId) : IRequest;

    public class AddCollectionToUserHandler : IRequestHandler<AddCollectionToUserCommand>
    {
        private readonly AppDbContext _db;

        public AddCollectionToUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddCollectionToUserCommand request, CancellationToken ct)
        {
            var exist = await _db.UserCollections
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type &&
                    x.ItemId == request.ItemId);

            if (exist) return Unit.Value;

            await _db.CreateEntity(new UserCollection
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Type = request.Type,
                ItemId = request.ItemId,
                CreatedAt = DateTimeOffset.UtcNow
            });

            return Unit.Value;
        }
    }
}
