using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.CommunityDesc.Queries
{
    public record GetContentAuthorVotesCountQuery(long UserId, VoteType Vote) : IRequest<uint>;

    public class GetContentAuthorVotesCountHandler : IRequestHandler<GetContentAuthorVotesCountQuery, uint>
    {
        private readonly AppDbContext _db;

        public GetContentAuthorVotesCountHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<uint> Handle(GetContentAuthorVotesCountQuery request, CancellationToken ct)
        {
            return (uint) await _db.ContentVotes
                .Include(x => x.Message)
                .CountAsync(x =>
                    x.Message.UserId == request.UserId &&
                    x.Vote == request.Vote &&
                    x.IsActive);
        }
    }
}
