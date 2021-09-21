using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Statistic.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Currency.Commands
{
    public record AddCurrencyToUserCommand(long UserId, CurrencyType Currency, uint Amount) : IRequest;

    public class AddCurrencyToUserHandler : IRequestHandler<AddCurrencyToUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddCurrencyToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddCurrencyToUserHandler(
            DbContextOptions options,
            IMediator mediator,
            ILogger<AddCurrencyToUserHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddCurrencyToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserCurrencies
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Currency == request.Currency);

            if (entity is null)
            {
                await _db.CreateEntity(new UserCurrency
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Currency = request.Currency,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user currency entity for user {UserId} with currency {Currency} and amount {Amount}",
                    request.UserId, request.Currency.ToString(), request.Amount);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} currency {Currency} amount {Amount}",
                    request.UserId, request.Currency.ToString(), request.Amount);
            }

            return await _mediator.Send(new AddStatisticToUserCommand(
                request.UserId, StatisticType.CurrencyEarned, request.Amount));
        }
    }
}
