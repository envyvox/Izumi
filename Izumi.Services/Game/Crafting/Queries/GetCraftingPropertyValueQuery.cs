using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crafting.Queries
{
    public record GetCraftingPropertyValueQuery(Guid CraftingId, CraftingPropertyType Property) : IRequest<uint>;

    public class GetCraftingPropertyValueHandler : IRequestHandler<GetCraftingPropertyValueQuery, uint>
    {
        private readonly AppDbContext _db;

        public GetCraftingPropertyValueHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<uint> Handle(GetCraftingPropertyValueQuery request, CancellationToken cancellationToken)
        {
            var entity = await _db.CraftingProperties
                .SingleOrDefaultAsync(x =>
                    x.CraftingId == request.CraftingId &&
                    x.Property == request.Property);

            if (entity is null)
            {
                throw new Exception(
                    $"property {request.Property.ToString()} for crafting {request.CraftingId} not found");
            }

            return entity.Value;
        }
    }
}
