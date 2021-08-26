using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Drink.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Drink.Queries
{
    public record GetDrinksQuery : IRequest<List<DrinkDto>>;

    public class GetDrinksHandler : IRequestHandler<GetDrinksQuery, List<DrinkDto>>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetDrinksHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<DrinkDto>> Handle(GetDrinksQuery request, CancellationToken ct)
        {
            var entities = await _db.Drinks
                .ToListAsync();

            return _mapper.Map<List<DrinkDto>>(entities);
        }
    }
}
