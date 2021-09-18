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

namespace Izumi.Services.Game.Crafting.Commands
{
    public record CreateCraftingIngredientsCommand(
            string CraftingName,
            List<CreateIngredientDto> Ingredients)
        : IRequest;

    public class CreateCraftingIngredientsHandler : IRequestHandler<CreateCraftingIngredientsCommand>
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public CreateCraftingIngredientsHandler(
            DbContextOptions options,
            IMediator mediator)
        {
            _mediator = mediator;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateCraftingIngredientsCommand request, CancellationToken ct)
        {
            var crafting = await _db.Craftings
                .SingleOrDefaultAsync(x => x.Name == request.CraftingName);

            if (crafting is null)
            {
                throw new Exception($"crafting with name {request.CraftingName} not found");
            }

            foreach (var createIngredient in request.Ingredients)
            {
                var ingredient = await _mediator.Send(new GetIngredientByNameQuery(
                    createIngredient.Category, createIngredient.Name));
                var exist = await _db.CraftingIngredients
                    .AnyAsync(x =>
                        x.CraftingId == crafting.Id &&
                        x.Category == ingredient.Category &&
                        x.IngredientId == ingredient.Id);

                if (exist)
                {
                    throw new Exception(
                        $"crafting {crafting.Name} already have ingredient with category {ingredient.Category.ToString()} and name {ingredient.Name}");
                }

                await _db.CreateEntity(new CraftingIngredient
                {
                    Id = Guid.NewGuid(),
                    CraftingId = crafting.Id,
                    Category = ingredient.Category,
                    IngredientId = ingredient.Id,
                    Amount = createIngredient.Amount
                });
            }

            return Unit.Value;
        }
    }
}
