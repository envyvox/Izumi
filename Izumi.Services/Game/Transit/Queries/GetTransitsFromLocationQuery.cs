﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Transit.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Transit.Queries
{
    public record GetTransitsFromLocationQuery(LocationType Departure) : IRequest<List<TransitDto>>;

    public class GetTransitsFromLocationHandler : IRequestHandler<GetTransitsFromLocationQuery, List<TransitDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetTransitsFromLocationHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<TransitDto>> Handle(GetTransitsFromLocationQuery request, CancellationToken ct)
        {
            var entities = await _db.Transits
                .AsQueryable()
                .Where(x => x.Departure == request.Departure)
                .ToListAsync();

            return _mapper.Map<List<TransitDto>>(entities);
        }
    }
}
