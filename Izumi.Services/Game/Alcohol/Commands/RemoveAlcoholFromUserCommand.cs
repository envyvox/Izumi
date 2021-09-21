using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Alcohol.Commands
{
    public record RemoveAlcoholFromUserCommand(long UserId, Guid AlcoholId, uint Amount) : IRequest;

    public class RemoveAlcoholFromUserHandler : IRequestHandler<RemoveAlcoholFromUserCommand>
    {
        private readonly ILogger<RemoveAlcoholFromUserHandler> _logger;
        private readonly AppDbContext _db;

        public RemoveAlcoholFromUserHandler(
            DbContextOptions options,
            ILogger<RemoveAlcoholFromUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveAlcoholFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserAlcohols
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.AlcoholId == request.AlcoholId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have alcohol {request.AlcoholId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed from user {UserId} alcohol {AlcoholId} amount {Amount}",
                request.UserId, request.AlcoholId, request.Amount);

            return Unit.Value;
        }
    }
}
