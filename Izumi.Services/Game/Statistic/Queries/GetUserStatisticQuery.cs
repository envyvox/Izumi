﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Statistic.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Statistic.Queries
{
    public record GetUserStatisticQuery(long UserId, StatisticType Type) : IRequest<UserStatisticDto>;

    public class GetUserStatisticHandler : IRequestHandler<GetUserStatisticQuery, UserStatisticDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserStatisticHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserStatisticDto> Handle(GetUserStatisticQuery request, CancellationToken ct)
        {
            var entity = await _db.UserStatistics
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            return entity is null
                ? new UserStatisticDto(request.Type, 0)
                : _mapper.Map<UserStatisticDto>(entity);
        }
    }
}
