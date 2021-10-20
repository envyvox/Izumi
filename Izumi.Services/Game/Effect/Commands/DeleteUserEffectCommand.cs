using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Effect.Commands
{
    public record DeleteUserEffectCommand(long UserId, EffectType Type) : IRequest;

    public class DeleteUserEffectHandler : IRequestHandler<DeleteUserEffectCommand>
    {
        private readonly ILogger<DeleteUserEffectHandler> _logger;
        private readonly AppDbContext _db;

        public DeleteUserEffectHandler(
            DbContextOptions options,
            ILogger<DeleteUserEffectHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteUserEffectCommand request, CancellationToken ct)
        {
            var entity = await _db.UserEffects
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have effect {request.Type.ToString()}");
            }

            await _db.DeleteEntity(entity);

            _logger.LogInformation(
                "Deleted user effect entity for user {UserId} and effect {Type}",
                request.UserId, request.Type.ToString());

            return Unit.Value;
        }
    }
}
