using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Effect.Commands
{
    public record DeleteUsersEffectCommand(EffectType Type) : IRequest;

    public class DeleteUsersEffectHandler : IRequestHandler<DeleteUsersEffectCommand>
    {
        private readonly ILogger<DeleteUsersEffectHandler> _logger;
        private readonly AppDbContext _db;

        public DeleteUsersEffectHandler(
            DbContextOptions options,
            ILogger<DeleteUsersEffectHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteUsersEffectCommand request, CancellationToken ct)
        {
            await _db.Database.ExecuteSqlInterpolatedAsync(
                $"delete from user_effects where type = {request.Type};");

            _logger.LogInformation(
                "Deleted all user effect entities with effect {Type}",
                request.Type.ToString());

            return Unit.Value;
        }
    }
}
