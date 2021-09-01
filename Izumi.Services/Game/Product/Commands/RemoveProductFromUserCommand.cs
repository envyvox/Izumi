using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Product.Commands
{
    public record RemoveProductFromUserCommand(long UserId, Guid ProductId, uint Amount) : IRequest;

    public class RemoveProductFromUserHandler : IRequestHandler<RemoveProductFromUserCommand>
    {
        private readonly AppDbContext _db;

        public RemoveProductFromUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveProductFromUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.UserProducts
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.ProductId == request.ProductId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have product {request.ProductId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
