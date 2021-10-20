using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Effect.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Effect.Commands
{
    public record CreateUserEffectCommand(long UserId, EffectType Type, DateTimeOffset? Expiration) : IRequest;

    public class CreateUserEffectHandler : IRequestHandler<CreateUserEffectCommand>
    {
        private readonly ILogger<CreateUserEffectHandler> _logger;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public CreateUserEffectHandler(
            DbContextOptions options,
            ILogger<CreateUserEffectHandler> logger,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateUserEffectCommand request, CancellationToken ct)
        {
            var exist = await _db.UserEffects
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (exist)
            {
                throw new Exception($"user {request.UserId} already have effect {request.Type.ToString()}");
            }

            await _db.CreateEntity(new UserEffect
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Type = request.Type,
                CreatedAt = DateTimeOffset.UtcNow,
                Expiration = request.Expiration
            });

            _logger.LogInformation(
                "Created user effect entity for user {UserId} with effect {Type} and expiration {Expiration}",
                request.UserId, request.Type, request.Expiration);

            if (request.Type is EffectType.Lottery) await CheckLottery();

            return Unit.Value;
        }

        private async Task CheckLottery()
        {
            var lotteryUsers = await _mediator.Send(new GetUsersWithEffectCountQuery(
                EffectType.Lottery));
            var requiredCount = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.CasinoLotteryRequiredUsersCount));

            if (lotteryUsers >= requiredCount) await _mediator.Send(new StartLotteryCommand());
        }
    }
}
