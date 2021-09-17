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
    public record UpdateDrinkCommand(Guid Id, string Name) : IRequest<DrinkDto>;

    public class UpdateDrinkHandler : IRequestHandler<UpdateDrinkCommand, DrinkDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateDrinkHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<DrinkDto> Handle(UpdateDrinkCommand request, CancellationToken ct)
        {
            var entity = await _db.Drinks
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.Name = request.Name;

            await _db.UpdateEntity(entity);

            return _mapper.Map<DrinkDto>(entity);
        }
    }
}
