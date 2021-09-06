using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.User.Commands
{
    public record RemoveEnergyFromUserCommand(long UserId, uint Amount) : IRequest;

    public class RemoveEnergyFromUserHandler : IRequestHandler<RemoveEnergyFromUserCommand>
    {
        private readonly AppDbContext _db;

        public RemoveEnergyFromUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveEnergyFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.Users
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            entity.Energy -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
