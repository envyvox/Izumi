using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Commands
{
    public record CreateAlcoholPropertyCommand(
            Guid AlcoholId,
            AlcoholPropertyType Property,
            uint Value)
        : IRequest;

    public class CreateAlcoholPropertyHandler : IRequestHandler<CreateAlcoholPropertyCommand>
    {
        private readonly AppDbContext _db;

        public CreateAlcoholPropertyHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateAlcoholPropertyCommand request, CancellationToken ct)
        {
            var exist = await _db.AlcoholProperties
                .AnyAsync(x =>
                    x.AlcoholId == request.AlcoholId &&
                    x.Property == request.Property);

            if (exist)
            {
                throw new Exception(
                    $"property {request.Property.ToString()} for alcohol {request.AlcoholId} already exist");
            }

            await _db.CreateEntity(new AlcoholProperty
            {
                Id = Guid.NewGuid(),
                AlcoholId = request.AlcoholId,
                Property = request.Property,
                Value = request.Value
            });

            return Unit.Value;
        }
    }
}
