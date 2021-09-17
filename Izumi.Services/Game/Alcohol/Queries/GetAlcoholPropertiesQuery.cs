using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Alcohol.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Queries
{
    public record GetAlcoholPropertiesQuery(Guid AlcoholId) : IRequest<List<AlcoholPropertyDto>>;

    public class GetAlcoholPropertiesHandler : IRequestHandler<GetAlcoholPropertiesQuery, List<AlcoholPropertyDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetAlcoholPropertiesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<AlcoholPropertyDto>> Handle(GetAlcoholPropertiesQuery request, CancellationToken ct)
        {
            var entities = await _db.AlcoholProperties
                .Include(x => x.Alcohol)
                .Where(x => x.AlcoholId == request.AlcoholId)
                .ToListAsync();

            return _mapper.Map<List<AlcoholPropertyDto>>(entities);
        }
    }
}
