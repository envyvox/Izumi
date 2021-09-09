using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Queries
{
    public record GetAlcoholPropertyValueQuery(Guid AlcoholId, AlcoholPropertyType Property) : IRequest<uint>;

    public class GetAlcoholPropertyValueHandler : IRequestHandler<GetAlcoholPropertyValueQuery, uint>
    {
        private readonly AppDbContext _db;

        public GetAlcoholPropertyValueHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<uint> Handle(GetAlcoholPropertyValueQuery request, CancellationToken ct)
        {
            var entity = await _db.AlcoholProperties
                .SingleOrDefaultAsync(x =>
                    x.AlcoholId == request.AlcoholId &&
                    x.Property == request.Property);

            if (entity is null)
            {
                throw new Exception(
                    $"property {request.Property.ToString()} for alcohol {request.AlcoholId} not found");
            }

            return entity.Value;
        }
    }
}
