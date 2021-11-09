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
    public record GetDrinkByNameQuery(string Name) : IRequest<DrinkDto>;

    public class GetDrinkByNameHandler : IRequestHandler<GetDrinkByNameQuery, DrinkDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetDrinkByNameHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<DrinkDto> Handle(GetDrinkByNameQuery request, CancellationToken ct)
        {
            var entity = await _db.Drinks
                .Include(x => x.Ingredients)
                .SingleOrDefaultAsync(x => x.Name == request.Name);

            if (entity is null)
            {
                throw new Exception($"drink with name {request.Name} not found");
            }

            return _mapper.Map<DrinkDto>(entity);
        }
    }
}