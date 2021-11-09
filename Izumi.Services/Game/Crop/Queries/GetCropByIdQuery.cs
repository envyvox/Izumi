using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crop.Queries
{
    public record GetCropByIdQuery(Guid Id) : IRequest<CropDto>;

    public class GetCropHandler : IRequestHandler<GetCropByIdQuery, CropDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetCropHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<CropDto> Handle(GetCropByIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Crops
                .Include(x => x.Seed)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"crop {request.Id} not found");
            }

            return _mapper.Map<CropDto>(entity);
        }
    }
}
