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
    public record StartReGrowthOnUserFieldCommand(long UserId, uint Number) : IRequest;

    public class StartReGrowthOnUserFieldHandler : IRequestHandler<StartReGrowthOnUserFieldCommand>
    {
        private readonly ILogger<StartReGrowthOnUserFieldHandler> _logger;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public StartReGrowthOnUserFieldHandler(
            DbContextOptions options,
            ILogger<StartReGrowthOnUserFieldHandler> logger,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(StartReGrowthOnUserFieldCommand request, CancellationToken ct)
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

            entity.Progress = 0;
            entity.InReGrowth = true;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.State = weather == WeatherType.Clear
                ? FieldStateType.Planted
                : FieldStateType.Watered;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Started re growth on user {UserId} field {Number}",
                request.UserId, request.Number);

            return Unit.Value;
        }
    }
}
