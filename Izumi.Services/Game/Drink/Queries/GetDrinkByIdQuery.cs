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
    public record GetDrinkByIdQuery(Guid Id) : IRequest<DrinkDto>;

    public class GetDrinkHandler : IRequestHandler<GetDrinkByIdQuery, DrinkDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetDrinkHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<DrinkDto> Handle(GetDrinkByIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Drinks
                .Include(x => x.Ingredients)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"drink {request.Id} not found");
            }

            return _mapper.Map<DrinkDto>(entity);
        }
    }
}
