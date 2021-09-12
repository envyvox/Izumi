using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Statistic.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Currency.Commands
{
    public record RemoveCurrencyFromUserCommand(long UserId, CurrencyType Currency, uint Amount) : IRequest;

    public class RemoveCurrencyFromUserHandler : IRequestHandler<RemoveCurrencyFromUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public RemoveCurrencyFromUserHandler(
            DbContextOptions options,
            IMediator mediator)
        {
            _mediator = mediator;
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

            return await _mediator.Send(new AddStatisticToUserCommand(
                request.UserId, StatisticType.CurrencySpent, request.Amount));
        }
    }
}
