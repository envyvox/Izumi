using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Transit.Commands
{
    public record CreateTransitCommand(
            LocationType Departure,
            LocationType Destination,
            TimeSpan Duration,
            uint Price)
        : IRequest;

    public class CreateTransitHandler : IRequestHandler<CreateTransitCommand>
    {
        private readonly AppDbContext _db;

        public CreateTransitHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateTransitCommand request, CancellationToken ct)
        {
            var exist = await _db.Transits
                .AnyAsync(x =>
                    x.Departure == request.Departure &&
                    x.Destination == request.Destination);

            if (exist)
            {
                throw new Exception(
                    $"transit from {request.Departure.ToString()} to {request.Destination.ToString()} already exist");
            }

            await _db.CreateEntity(new Data.Entities.Transit
            {
                Id = Guid.NewGuid(),
                Departure = request.Departure,
                Destination = request.Destination,
                Duration = request.Duration,
                Price = request.Price
            });

            return Unit.Value;
        }
    }
}
