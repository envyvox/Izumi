using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.User.Commands
{
    public record AddEnergyToUserCommand(long UserId, uint Amount) : IRequest;

    public class AddEnergyToUserHandler : IRequestHandler<AddEnergyToUserCommand>
    {
        private readonly ILogger<AddEnergyToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddEnergyToUserHandler(
            DbContextOptions options,
            ILogger<AddEnergyToUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddEnergyToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.Users
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            entity.Energy = entity.Energy + request.Amount > 100
                ? 100
                : entity.Energy + request.Amount;

            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Added user {UserId} energy amount {Amount}",
                request.UserId, request.Amount);

            return Unit.Value;
        }
    }
}
