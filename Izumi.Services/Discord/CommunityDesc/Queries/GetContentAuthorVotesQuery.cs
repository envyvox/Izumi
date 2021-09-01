﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Discord.CommunityDesc.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.CommunityDesc.Queries
{
    public record GetContentAuthorVotesQuery(long UserId) : IRequest<List<ContentVoteDto>>;

    public class GetContentAuthorVotesHandler : IRequestHandler<GetContentAuthorVotesQuery, List<ContentVoteDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetContentAuthorVotesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<ContentVoteDto>> Handle(GetContentAuthorVotesQuery request, CancellationToken ct)
        {
            var entities = await _db.ContentVotes
                .Include(x => x.Message)
                .Where(x =>
                    x.Message.UserId == request.UserId &&
                    x.IsActive)
                .ToListAsync();

            return _mapper.Map<List<ContentVoteDto>>(entities);
        }
    }
}
