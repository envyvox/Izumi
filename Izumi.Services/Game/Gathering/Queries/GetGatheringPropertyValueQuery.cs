using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Gathering.Queries
{
    public record GetGatheringPropertyValueQuery(Guid GatheringId, GatheringPropertyType Property) : IRequest<uint>;

    public class GetGatheringPropertyValueHandler : IRequestHandler<GetGatheringPropertyValueQuery, uint>
    {
        private readonly AppDbContext _db;

        public GetGatheringPropertyValueHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<uint> Handle(GetGatheringPropertyValueQuery request, CancellationToken ct)
        {
            var entity = await _db.GatheringProperties
                .SingleOrDefaultAsync(x =>
                    x.GatheringId == request.GatheringId &&
                    x.Property == request.Property);

            if (entity is null)
            {
                throw new Exception(
                    $"property {request.Property.ToString()} for gathering {request.GatheringId} not found");
            }

            return entity.Value;
        }
    }
}
