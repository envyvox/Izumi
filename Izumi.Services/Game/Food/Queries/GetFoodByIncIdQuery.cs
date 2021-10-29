using System;
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
    public record GetFoodByIncIdQuery(long IncId) : IRequest<FoodDto>;

    public class GetFoodByIncIdHandler : IRequestHandler<GetFoodByIncIdQuery, FoodDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetFoodByIncIdHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(GetFoodByIncIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _db.Foods
                .Include(x => x.Ingredients)
                .SingleOrDefaultAsync(x => x.AutoIncrementedId == request.IncId);

            if (entity is null)
            {
                throw new GameUserExpectedException("никогда не слышала о блюде с таким номером");
            }

            return _mapper.Map<FoodDto>(entity);
        }
    }
}
