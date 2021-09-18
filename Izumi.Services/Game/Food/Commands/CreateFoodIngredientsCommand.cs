using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Ingredient.Models;
using Izumi.Services.Game.Ingredient.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Food.Commands
{
    public record CreateFoodIngredientsCommand(string FoodName, List<CreateIngredientDto> Ingredients) : IRequest;

    public class CreateFoodIngredientsHandler : IRequestHandler<CreateFoodIngredientsCommand>
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public CreateFoodIngredientsHandler(
            DbContextOptions options,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateFoodIngredientsCommand request, CancellationToken ct)
        {
            var food = await _db.Foods
                .SingleOrDefaultAsync(x => x.Name == request.FoodName);

            if (food is null)
            {
                throw new Exception($"food with name {request.FoodName} not found");
            }

            foreach (var createIngredient in request.Ingredients)
            {
                var ingredient = await _mediator.Send(new GetIngredientByNameQuery(
                    createIngredient.Category, createIngredient.Name));
                var exist = await _db.FoodIngredients
                    .AnyAsync(x =>
                        x.FoodId == food.Id &&
                        x.Category == ingredient.Category &&
                        x.IngredientId == ingredient.Id);

                if (exist)
                {
                    throw new Exception(
                        $"food {food.Name} already have ingredient with category {ingredient.Category.ToString()} and name {ingredient.Name}");
                }

                await _db.CreateEntity(new FoodIngredient
                {
                    Id = Guid.NewGuid(),
                    FoodId = food.Id,
                    Category = ingredient.Category,
                    IngredientId = ingredient.Id,
                    Amount = createIngredient.Amount
                });
            }

            return Unit.Value;
        }
    }
}
