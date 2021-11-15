using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Food.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Food.Commands
{
    public record UpdateFoodCommand(
            Guid Id,
            string Name,
            bool RecipeSellable,
            bool IsSpecial)
        : IRequest<FoodDto>;

    public class UpdateFoodHandler : IRequestHandler<UpdateFoodCommand, FoodDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateFoodHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(UpdateFoodCommand request, CancellationToken ct)
        {
            var entity = await _db.Foods
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.Name = request.Name;
            entity.RecipeSellable = request.RecipeSellable;
            entity.IsSpecial = request.IsSpecial;

            await _db.UpdateEntity(entity);

            return _mapper.Map<FoodDto>(entity);
        }
    }
}
