using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Discord.CommunityDesc.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.CommunityDesc.Queries
{
    public record GetUserVotesOnMessageQuery(
            long UserId,
            Guid ContentMessageId)
        : IRequest<Dictionary<VoteType, ContentVoteDto>>;

    public class GetUserVotesOnMessageHandler
        : IRequestHandler<GetUserVotesOnMessageQuery, Dictionary<VoteType, ContentVoteDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserVotesOnMessageHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<Dictionary<VoteType, ContentVoteDto>> Handle(GetUserVotesOnMessageQuery request,
            CancellationToken ct)
        {
            var entities = await _db.ContentVotes
                .Include(x => x.User)
                .Include(x => x.Message)
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.MessageId == request.ContentMessageId)
                .ToDictionaryAsync(x => x.Vote);

            return _mapper.Map<Dictionary<VoteType, ContentVoteDto>>(entities);
        }
    }
}
