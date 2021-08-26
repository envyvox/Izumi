using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Banner.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Banner.Queries
{
    public record GetBannerByIncIdQuery(long IncId) : IRequest<BannerDto>;

    public class GetBannerByIncIdHandler : IRequestHandler<GetBannerByIncIdQuery, BannerDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetBannerByIncIdHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _mapper = mapper;
            _db = new AppDbContext(options);
        }

        public async Task<BannerDto> Handle(GetBannerByIncIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Banners
                .SingleOrDefaultAsync(x => x.AutoIncrementedId == request.IncId);

            if (entity is null)
            {
                throw new Exception($"banner with inc id {request.IncId} not found");
            }

            return _mapper.Map<BannerDto>(entity);
        }
    }
}
