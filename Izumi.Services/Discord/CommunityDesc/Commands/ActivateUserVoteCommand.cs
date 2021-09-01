using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.CommunityDesc.Commands
{
    public record ActivateUserVoteCommand(long UserId, Guid ContentMessageId, VoteType Vote) : IRequest;

    public class ActivateUserVoteHandler : IRequestHandler<ActivateUserVoteCommand>
    {
        private readonly AppDbContext _db;

        public ActivateUserVoteHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(ActivateUserVoteCommand request, CancellationToken ct)
        {
            var entity = await _db.ContentVotes
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.MessageId == request.ContentMessageId &&
                    x.Vote == request.Vote);

            if (entity is null)
            {
                throw new Exception(
                    $"content vote with params userId: {request.UserId}, messageId: {request.ContentMessageId}, vote: {request.Vote} not found");
            }

            entity.IsActive = true;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
