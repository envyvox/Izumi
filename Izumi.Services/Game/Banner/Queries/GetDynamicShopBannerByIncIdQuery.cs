using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Banner.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Izumi.Services.Extensions.ExceptionExtensions;

namespace Izumi.Services.Game.Banner.Queries
{
    public record GetDynamicShopBannerByIncIdQuery(long IncId) : IRequest<BannerDto>;

    public class GetDynamicShopBannerByIncIdHandler : IRequestHandler<GetDynamicShopBannerByIncIdQuery, BannerDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetDynamicShopBannerByIncIdHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<BannerDto> Handle(GetDynamicShopBannerByIncIdQuery request, CancellationToken ct)
        {
            var entity = await _db.DynamicShopBanners
                .Include(x => x.Banner)
                .Select(x => x.Banner)
                .SingleOrDefaultAsync(x => x.AutoIncrementedId == request.IncId);

            if (entity is null)
            {
                throw new GameUserExpectedException("в магазине нет баннера с таким номером.");
            }

            return _mapper.Map<BannerDto>(entity);
        }
    }
}
