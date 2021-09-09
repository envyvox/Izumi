using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crafting.Commands
{
    public record CreateCraftingPropertyCommand(
            Guid CraftingId,
            CraftingPropertyType Property,
            uint Value)
        : IRequest;

    public class CreateCraftingPropertyHandler : IRequestHandler<CreateCraftingPropertyCommand>
    {
        private readonly AppDbContext _db;

        public CreateCraftingPropertyHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateCraftingPropertyCommand request, CancellationToken ct)
        {
            var exist = await _db.CraftingProperties
                .AnyAsync(x =>
                    x.CraftingId == request.CraftingId &&
                    x.Property == request.Property);

            if (exist)
            {
                throw new Exception(
                    $"property {request.Property.ToString()} for crafting {request.CraftingId} already exist");
            }

            await _db.CreateEntity(new CraftingProperty
            {
                Id = Guid.NewGuid(),
                CraftingId = request.CraftingId,
                Property = request.Property,
                Value = request.Value
            });

            return Unit.Value;
        }
    }
}
