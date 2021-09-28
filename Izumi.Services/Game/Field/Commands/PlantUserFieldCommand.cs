using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.World.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Field.Commands
{
    public record PlantUserFieldCommand(long UserId, uint Number, Guid SeedId) : IRequest;

    public class PlantUserFieldHandler : IRequestHandler<PlantUserFieldCommand>
    {
        private readonly ILogger<PlantUserFieldHandler> _logger;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public PlantUserFieldHandler(
            DbContextOptions options,
            ILogger<PlantUserFieldHandler> logger,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(PlantUserFieldCommand request, CancellationToken ct)
        {
            var entity = await _db.UserFields
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Number == request.Number);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have field with number {request.Number}");
            }

            var weather = await _mediator.Send(new GetWeatherTodayQuery());
            var state = weather == WeatherType.Clear
                ? FieldStateType.Planted
                : FieldStateType.Watered;

            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.SeedId = request.SeedId;
            entity.State = state;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Planted user {UserId} field {Number} with seed {SeedId} and state {State}",
                request.UserId, request.Number, request.SeedId, state.ToString());

            return Unit.Value;
        }
    }
}
