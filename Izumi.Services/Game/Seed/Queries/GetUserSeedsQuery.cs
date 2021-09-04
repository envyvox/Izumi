﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seed.Queries
{
    public record GetUserSeedsQuery(long UserId) : IRequest<List<UserSeedDto>>;

    public class GetUserSeedsHandler : IRequestHandler<GetUserSeedsQuery, List<UserSeedDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserSeedsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserSeedDto>> Handle(GetUserSeedsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserSeeds
                .AmountNotZero()
                .Include(x => x.Seed)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserSeedDto>>(entities);
        }
    }
}
