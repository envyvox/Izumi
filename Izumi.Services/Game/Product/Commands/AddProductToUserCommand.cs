using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Product.Commands
{
    public record AddProductToUserCommand(long UserId, Guid ProductId, uint Amount) : IRequest;

    public class AddProductToUserHandler : IRequestHandler<AddProductToUserCommand>
    {
        private readonly AppDbContext _db;

        public AddProductToUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddProductToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserProducts
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.ProductId == request.ProductId);

            if (entity is null)
            {
                await _db.CreateEntity(new UserProduct
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    ProductId = request.ProductId,
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
