using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Food.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Food.Queries
{
    public record GetFoodsQuery : IRequest<List<FoodDto>>;

    public class GetFoodsHandler : IRequestHandler<GetFoodsQuery, List<FoodDto>>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetFoodsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<FoodDto>> Handle(GetFoodsQuery request, CancellationToken ct)
        {
            var entities = await _db.Foods
                .Include(x => x.Ingredients)
                .ToListAsync();

            return _mapper.Map<List<FoodDto>>(entities);
        }
    }
}
