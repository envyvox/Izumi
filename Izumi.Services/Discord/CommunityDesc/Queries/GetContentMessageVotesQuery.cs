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
    public record GetContentMessageVotesQuery(Guid ContentMessageId, VoteType Vote) : IRequest<List<ContentVoteDto>>;

    public class GetContentMessageVotesHandler : IRequestHandler<GetContentMessageVotesQuery, List<ContentVoteDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetContentMessageVotesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<ContentVoteDto>> Handle(GetContentMessageVotesQuery request, CancellationToken ct)
        {
            var entities = await _db.ContentVotes
                .Include(x => x.User)
                .Include(x => x.Message)
                .Where(x =>
                    x.MessageId == request.ContentMessageId &&
                    x.Vote == request.Vote &&
                    x.IsActive)
                .ToListAsync();

            return _mapper.Map<List<ContentVoteDto>>(entities);
        }
    }
}
