using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Food.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Food.Commands
{
    public record CreateFoodCommand(
            string Name,
            FoodCategoryType Category,
            bool RecipeSellable,
            bool IsSpecial)
        : IRequest<FoodDto>;

    public class CreateFoodHandler : IRequestHandler<CreateFoodCommand, FoodDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CreateFoodHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(CreateFoodCommand request, CancellationToken ct)
        {
            var exist = await _db.Foods
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"food with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Resource.Food
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Category = request.Category,
                RecipeSellable = request.RecipeSellable,
                IsSpecial = request.IsSpecial
            });

            return _mapper.Map<FoodDto>(created);
        }
    }
}
