using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Drink.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Drink.Queries
{
    public record GetUserDrinkQuery(long UserId, Guid DrinkId) : IRequest<UserDrinkDto>;

    public class GetUserDrinkHandler : IRequestHandler<GetUserDrinkQuery, UserDrinkDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserDrinkHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserDrinkDto> Handle(GetUserDrinkQuery request, CancellationToken ct)
        {
            var entity = await _db.UserDrinks
                .Include(x => x.Drink)
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.DrinkId == request.DrinkId);

            return entity is null
                ? new UserDrinkDto(null, 0)
                : _mapper.Map<UserDrinkDto>(entity);
        }
    }
}
