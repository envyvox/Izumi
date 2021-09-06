using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.World.Commands
{
    public record UpdateWorldPropertyCommand(WorldPropertyType Type, uint Value) : IRequest;

    public class UpdateWorldPropertyHandler : IRequestHandler<UpdateWorldPropertyCommand>
    {
        private readonly AppDbContext _db;

        public UpdateWorldPropertyHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateWorldPropertyCommand request, CancellationToken ct)
        {
            var entity = await _db.WorldProperties
                .SingleOrDefaultAsync(x => x.Type == request.Type);

            if (entity is null)
            {
                await _db.CreateEntity(new WorldProperty
                {
                    Type = request.Type,
                    Value = request.Value
                });
            }
            else
            {
                entity.Value = request.Value;

                await _db.UpdateEntity(entity);
            }

            return Unit.Value;
        }
    }
}
