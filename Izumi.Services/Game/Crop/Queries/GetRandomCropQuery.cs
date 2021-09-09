using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crop.Queries
{
    public record GetRandomCropQuery : IRequest<CropDto>;

    public class GetRandomCropHandler : IRequestHandler<GetRandomCropQuery, CropDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetRandomCropHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<CropDto> Handle(GetRandomCropQuery request, CancellationToken ct)
        {
            var entity = await _db.Crops
                .OrderByRandom()
                .FirstOrDefaultAsync();

            return _mapper.Map<CropDto>(entity);
        }
    }
}
