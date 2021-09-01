using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.Discord;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.CommunityDesc.Commands
{
    public record CreateUserVoteCommand(long UserId, Guid ContentMessageId, VoteType Vote) : IRequest;

    public class CreateUserVoteHandler : IRequestHandler<CreateUserVoteCommand>
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public CreateUserVoteHandler(
            DbContextOptions options,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateUserVoteCommand request, CancellationToken cancellationToken)
        {
            var exist = await _db.ContentVotes
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.MessageId == request.ContentMessageId);

            if (exist)
            {
                throw new Exception(
                    $"vote from user {request.UserId} on content message {request.ContentMessageId} already exist");
            }

            await _db.CreateEntity(new ContentVote
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                MessageId = request.ContentMessageId,
                Vote = request.Vote,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            if (request.Vote == VoteType.Like)
                await _mediator.Send(new CheckContentMessageLikesCommand(request.ContentMessageId));
            if (request.Vote == VoteType.Dislike)
                await _mediator.Send(new CheckContentMessageDislikesCommand(request.ContentMessageId));

            return Unit.Value;
        }
    }
}
