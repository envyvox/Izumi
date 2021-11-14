using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Food.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Izumi.Services.Extensions.ExceptionExtensions;

namespace Izumi.Services.Game.Food.Queries
{
    public record GetDynamicShopRecipeByIncIdQuery(long IncId) : IRequest<FoodDto>;

    public class GetDynamicShopRecipeByIncIdHandler : IRequestHandler<GetDynamicShopRecipeByIncIdQuery, FoodDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetDynamicShopRecipeByIncIdHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(GetDynamicShopRecipeByIncIdQuery request, CancellationToken ct)
        {
            var entity = await _db.DynamicShopRecipes
                .Include(x => x.Food)
                .ThenInclude(x => x.Ingredients)
                .Where(x => x.Food.AutoIncrementedId == request.IncId)
                .Select(x => x.Food)
                .SingleOrDefaultAsync();

            if (entity is null)
            {
                throw new GameUserExpectedException("в магазине нет рецепта с таким номером.");
            }

            return _mapper.Map<FoodDto>(entity);
        }
    }
}