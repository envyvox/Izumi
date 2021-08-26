using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Drink.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Drink.Commands
{
    public record CreateDrinkCommand(string Name) : IRequest<DrinkDto>;

    public class CreateDrinkHandler : IRequestHandler<CreateDrinkCommand, DrinkDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CreateDrinkHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<DrinkDto> Handle(CreateDrinkCommand request, CancellationToken ct)
        {
            var exist = await _db.Drinks
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"drink with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Resource.Drink
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            });

            return _mapper.Map<DrinkDto>(created);
        }
    }
}
