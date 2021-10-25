using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Currency.Commands
{
    public record RemoveCurrencyFromUserCommand(long UserId, CurrencyType Currency, uint Amount) : IRequest;

    public class RemoveCurrencyFromUserHandler : IRequestHandler<RemoveCurrencyFromUserCommand>
    {
        private readonly ILogger<RemoveCurrencyFromUserHandler> _logger;
        private readonly AppDbContext _db;

        public RemoveCurrencyFromUserHandler(
            DbContextOptions options,
            ILogger<RemoveCurrencyFromUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveCurrencyFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserCurrencies
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Currency == request.Currency);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have currency {request.Currency}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed from user {UserId} currency {Currency} amount {Amount}",
                request.UserId, request.Currency, request.Amount);

            return Unit.Value;
        }
    }
}
