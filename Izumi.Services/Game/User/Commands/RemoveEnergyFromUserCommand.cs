using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Statistic.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.User.Commands
{
    public record RemoveEnergyFromUserCommand(long UserId, uint Amount) : IRequest;

    public class RemoveEnergyFromUserHandler : IRequestHandler<RemoveEnergyFromUserCommand>
    {
        private readonly ILogger<RemoveEnergyFromUserHandler> _logger;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public RemoveEnergyFromUserHandler(
            DbContextOptions options,
            ILogger<RemoveEnergyFromUserHandler> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveEnergyFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.Users
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            var energySpent = request.Amount > entity.Energy
                ? entity.Energy
                : entity.Energy - request.Amount;

            entity.Energy -= energySpent;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed energy from user {UserId} amount {EnergySpent} (requested value was {Amount})",
                request.UserId, energySpent, request.Amount);

            await _mediator.Send(new AddStatisticToUserCommand(entity.Id, StatisticType.EnergySpent, energySpent));

            return Unit.Value;
        }
    }
}
