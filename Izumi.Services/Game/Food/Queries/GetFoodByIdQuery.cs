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
    public record GetFoodByIdQuery(Guid Id) : IRequest<FoodDto>;

    public class GetFoodHandler : IRequestHandler<GetFoodByIdQuery, FoodDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetFoodHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(GetFoodByIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Foods
                .Include(x => x.Ingredients)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"food {request.Id} not found");
            }

            return _mapper.Map<FoodDto>(entity);
        }
    }
}
