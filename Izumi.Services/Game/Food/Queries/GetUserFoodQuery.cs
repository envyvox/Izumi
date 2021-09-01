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
    public record GetUserFoodQuery(long UserId, Guid FoodId) : IRequest<UserFoodDto>;

    public class GetUserFoodHandler : IRequestHandler<GetUserFoodQuery, UserFoodDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserFoodHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserFoodDto> Handle(GetUserFoodQuery request, CancellationToken ct)
        {
            var entity = await _db.UserFoods
                .Include(x => x.Food)
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.FoodId == request.FoodId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have food {request.FoodId}");
            }

            return _mapper.Map<UserFoodDto>(entity);
        }
    }
}
