using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Food.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Food.Queries
{
    public record GetFoodByNameQuery(string Name) : IRequest<FoodDto>;

    public class GetFoodByNameHandler : IRequestHandler<GetFoodByNameQuery, FoodDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetFoodByNameHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(GetFoodByNameQuery request, CancellationToken ct)
        {
            var entity = await _db.Foods
                .Include(x => x.Ingredients)
                .SingleOrDefaultAsync(x => x.Name == request.Name);

            if (entity is null)
            {
                throw new Exception($"food with name {request.Name} not found");
            }

            return _mapper.Map<FoodDto>(entity);
        }
    }
}