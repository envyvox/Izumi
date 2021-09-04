using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Food.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Food.Queries
{
    public record GetUserFoodsQuery(long UserId) : IRequest<List<UserFoodDto>>;

    public class GetUserFoodsHandler : IRequestHandler<GetUserFoodsQuery, List<UserFoodDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserFoodsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserFoodDto>> Handle(GetUserFoodsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserFoods
                .AmountNotZero()
                .Include(x => x.Food)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserFoodDto>>(entities);
        }
    }
}
