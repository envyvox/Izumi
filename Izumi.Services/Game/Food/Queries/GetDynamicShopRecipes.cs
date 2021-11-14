using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Food.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Food.Queries
{
    public record GetDynamicShopRecipesQuery : IRequest<List<FoodDto>>;

    public class GetDynamicShopRecipesHandler : IRequestHandler<GetDynamicShopRecipesQuery, List<FoodDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetDynamicShopRecipesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<FoodDto>> Handle(GetDynamicShopRecipesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _db.DynamicShopRecipes
                .Include(x => x.Food)
                .ThenInclude(x => x.Ingredients)
                .OrderBy(x => x.Food.AutoIncrementedId)
                .Select(x => x.Food)
                .ToListAsync();

            return _mapper.Map<List<FoodDto>>(entities);
        }
    }
}