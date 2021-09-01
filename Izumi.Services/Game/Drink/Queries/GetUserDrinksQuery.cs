using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Drink.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Drink.Queries
{
    public record GetUserDrinksQuery(long UserId) : IRequest<List<UserDrinkDto>>;

    public class GetUserDrinksHandler : IRequestHandler<GetUserDrinksQuery, List<UserDrinkDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserDrinksHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserDrinkDto>> Handle(GetUserDrinksQuery request, CancellationToken ct)
        {
            var entities = await _db.UserDrinks
                .Include(x => x.Drink)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserDrinkDto>>(entities);
        }
    }
}
