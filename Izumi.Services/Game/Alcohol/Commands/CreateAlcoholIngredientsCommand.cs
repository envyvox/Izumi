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

namespace Izumi.Services.Game.Alcohol.Commands
{
    public record CreateAlcoholIngredientsCommand(string AlcoholName, List<CreateIngredientDto> Ingredients) : IRequest;

    public class CreateAlcoholIngredientsHandler : IRequestHandler<CreateAlcoholIngredientsCommand>
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public CreateAlcoholIngredientsHandler(
            DbContextOptions options,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateAlcoholIngredientsCommand request, CancellationToken ct)
        {
            var alcohol = await _db.Alcohols
                .SingleOrDefaultAsync(x => x.Name == request.AlcoholName);

            if (alcohol is null)
            {
                throw new Exception($"alcohol with name {request.AlcoholName} not found");
            }

            foreach (var createIngredient in request.Ingredients)
            {
                var ingredient = await _mediator.Send(new GetIngredientByNameQuery(
                    createIngredient.Category, createIngredient.Name));
                var exist = await _db.AlcoholIngredients
                    .AnyAsync(x =>
                        x.AlcoholId == alcohol.Id &&
                        x.Category == ingredient.Category &&
                        x.IngredientId == ingredient.Id);

                if (exist)
                {
                    throw new Exception(
                        $"alcohol {alcohol.Name} already have ingredient with category {ingredient.Category.ToString()} and name {ingredient.Name}");
                }

                await _db.CreateEntity(new AlcoholIngredient
                {
                    Id = Guid.NewGuid(),
                    AlcoholId = alcohol.Id,
                    Category = ingredient.Category,
                    IngredientId = ingredient.Id,
                    Amount = createIngredient.Amount
                });
            }

            return Unit.Value;
        }
    }
}
