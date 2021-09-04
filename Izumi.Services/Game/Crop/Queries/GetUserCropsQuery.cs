using System.Collections.Generic;
using System.Linq;
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
    public record GetUserCropsQuery(long UserId) : IRequest<List<UserCropDto>>;

    public class GetUserCropsHandler : IRequestHandler<GetUserCropsQuery, List<UserCropDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserCropsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserCropDto>> Handle(GetUserCropsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserCrops
                .AmountNotZero()
                .Include(x => x.Crop)
                .ThenInclude(x => x.Seed)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserCropDto>>(entities);
        }
    }
}
