using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Emote.Commands
{
    public record CreateEmoteCommand(long Id, string Name, string Code) : IRequest;

    public class CreateEmoteHandler : IRequestHandler<CreateEmoteCommand>
    {
        private readonly ILogger<CreateEmoteHandler> _logger;
        private readonly AppDbContext _db;

        public CreateEmoteHandler(
            DbContextOptions options,
            ILogger<CreateEmoteHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateEmoteCommand request, CancellationToken ct)
        {
            var entity = await _db.Emotes
                .SingleOrDefaultAsync(x => x.Name == request.Name);

            if (entity is null)
            {
                await _db.CreateEntity(new Data.Entities.Discord.Emote
                {
                    Id = request.Id,
                    Name = request.Name,
                    Code = request.Code,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Added new emote {Code}",
                    request.Code);
            }
            else
            {
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);
            }

            return Unit.Value;
        }
    }
}
