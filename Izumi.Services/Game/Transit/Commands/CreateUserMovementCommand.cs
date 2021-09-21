using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Transit.Commands
{
    public record CreateUserMovementCommand(
            long UserId,
            LocationType Departure,
            LocationType Destination,
            DateTimeOffset Arrival)
        : IRequest;

    public class CreateUserMovementHandler : IRequestHandler<CreateUserMovementCommand>
    {
        private readonly ILogger<CreateUserMovementHandler> _logger;
        private readonly AppDbContext _db;

        public CreateUserMovementHandler(
            DbContextOptions options,
            ILogger<CreateUserMovementHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateUserMovementCommand request, CancellationToken ct)
        {
            var exist = await _db.UserMovements
                .AnyAsync(x => x.UserId == request.UserId);

            if (exist)
            {
                throw new Exception($"user {request.UserId} already have movement entity");
            }

            await _db.CreateEntity(new UserMovement
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Departure = request.Departure,
                Destination = request.Destination,
                Arrival = request.Arrival,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user movement entity for user {UserId} with departure {Departure} and destination {Destination}, arrival at {Arrival}",
                request.UserId, request.Departure, request.Destination, request.Arrival);

            return Unit.Value;
        }
    }
}
