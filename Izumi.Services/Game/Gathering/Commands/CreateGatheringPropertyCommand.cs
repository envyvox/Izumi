using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Gathering.Commands
{
    public record CreateGatheringPropertyCommand(
            Guid GatheringId,
            GatheringPropertyType Property,
            uint Value)
        : IRequest;

    public class CreateGatheringPropertyHandler : IRequestHandler<CreateGatheringPropertyCommand>
    {
        private readonly AppDbContext _db;

        public CreateGatheringPropertyHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateGatheringPropertyCommand request, CancellationToken ct)
        {
            var exist = await _db.GatheringProperties
                .AnyAsync(x =>
                    x.GatheringId == request.GatheringId &&
                    x.Property == request.Property);

            if (exist)
            {
                throw new Exception(
                    $"property {request.Property.ToString()} for gathering {request.GatheringId} already exist");
            }

            await _db.CreateEntity(new GatheringProperty
            {
                Id = Guid.NewGuid(),
                GatheringId = request.GatheringId,
                Property = request.Property,
                Value = request.Value
            });

            return Unit.Value;
        }
    }
}
