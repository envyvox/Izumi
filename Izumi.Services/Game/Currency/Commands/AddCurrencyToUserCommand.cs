using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Currency.Commands
{
    public record AddCurrencyToUserCommand(long UserId, CurrencyType Currency, uint Amount) : IRequest;

    public class AddCurrencyToUserHandler : IRequestHandler<AddCurrencyToUserCommand>
    {
        private readonly AppDbContext _db;

        public AddCurrencyToUserHandler(DbContextOptions options)
        {
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
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);
            }

            return Unit.Value;
        }
    }
}
