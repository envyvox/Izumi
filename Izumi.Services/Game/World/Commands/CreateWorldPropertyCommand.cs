using System;
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
    public record CreateWorldPropertyCommand(WorldPropertyType Type, uint Value) : IRequest;

    public class CreateWorldPropertyHandler : IRequestHandler<CreateWorldPropertyCommand>
    {
        private readonly AppDbContext _db;

        public CreateWorldPropertyHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateWorldPropertyCommand request, CancellationToken ct)
        {
            var exist = await _db.WorldProperties
                .AnyAsync(x => x.Type == request.Type);

            if (exist)
            {
                throw new Exception($"world property {request.Type.ToString()} already exist");
            }

            await _db.CreateEntity(new WorldProperty
            {
                Type = request.Type,
                Value = request.Value
            });

            return Unit.Value;
        }
    }
}
