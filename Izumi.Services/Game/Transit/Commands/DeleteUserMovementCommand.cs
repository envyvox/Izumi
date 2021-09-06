using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Transit.Commands
{
    public record DeleteUserMovementCommand(long UserId) : IRequest;

    public class DeleteUserMovementHandler : IRequestHandler<DeleteUserMovementCommand>
    {
        private readonly AppDbContext _db;

        public DeleteUserMovementHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(DeleteUserMovementCommand request, CancellationToken ct)
        {
            var entity = await _db.UserMovements
                .SingleOrDefaultAsync(x => x.UserId == request.UserId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have movement entity");
            }

            await _db.DeleteEntity(entity);

            return Unit.Value;
        }
    }
}
