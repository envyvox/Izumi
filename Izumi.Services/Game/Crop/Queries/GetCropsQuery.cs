using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crop.Queries
{
    public record GetCropsQuery : IRequest<List<CropDto>>;

    public class GetCropsHandler : IRequestHandler<GetCropsQuery, List<CropDto>>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetCropsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<CropDto>> Handle(GetCropsQuery request, CancellationToken ct)
        {
            var entities = await _db.Crops
                .Include(x => x.Seed)
                .ToListAsync();

            return _mapper.Map<List<CropDto>>(entities);
        }
    }
}
