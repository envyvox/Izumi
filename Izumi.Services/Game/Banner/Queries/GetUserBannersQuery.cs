using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Banner.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Banner.Queries
{
    public record GetUserBannersQuery(long UserId) : IRequest<List<UserBannerDto>>;

    public class GetUserBannersHandler : IRequestHandler<GetUserBannersQuery, List<UserBannerDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserBannersHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserBannerDto>> Handle(GetUserBannersQuery request, CancellationToken ct)
        {
            var entities = await _db.UserBanners
                .Include(x => x.Banner)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserBannerDto>>(entities);
        }
    }
}
