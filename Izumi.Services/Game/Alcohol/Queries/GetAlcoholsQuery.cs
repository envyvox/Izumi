using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Alcohol.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Queries
{
    public record GetAlcoholsQuery : IRequest<List<AlcoholDto>>;

    public class GetAlcoholsHandler : IRequestHandler<GetAlcoholsQuery, List<AlcoholDto>>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetAlcoholsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<AlcoholDto>> Handle(GetAlcoholsQuery request, CancellationToken ct)
        {
            var entities = await _db.Alcohols
                .Include(x => x.Properties)
                .ToListAsync();

            return _mapper.Map<List<AlcoholDto>>(entities);
        }
    }
}
