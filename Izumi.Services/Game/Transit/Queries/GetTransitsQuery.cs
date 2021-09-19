using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Transit.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Transit.Queries
{
    public record GetTransitsQuery : IRequest<List<TransitDto>>;

    public class GetTransitsHandler : IRequestHandler<GetTransitsQuery, List<TransitDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetTransitsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<TransitDto>> Handle(GetTransitsQuery request, CancellationToken ct)
        {
            var entities = await _db.Transits
                .AsQueryable()
                .OrderBy(x => x.Departure)
                .ThenBy(x => x.Destination)
                .ToListAsync();

            return _mapper.Map<List<TransitDto>>(entities);
        }
    }
}
