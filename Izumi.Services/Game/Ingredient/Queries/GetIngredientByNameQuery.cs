using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Ingredient.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Ingredient.Queries
{
    public record GetIngredientByNameQuery(IngredientCategoryType Category, string Name) : IRequest<IngredientDto>;

    public class GetIngredientByNameHandler : IRequestHandler<GetIngredientByNameQuery, IngredientDto>
    {
        private readonly AppDbContext _db;

        public GetIngredientByNameHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<IngredientDto> Handle(GetIngredientByNameQuery request, CancellationToken ct)
        {
            switch (request.Category)
            {
                case IngredientCategoryType.Gathering:
                {
                    var entity = await _db.Gatherings
                        .SingleOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"gathering with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }

                case IngredientCategoryType.Product:
                {
                    var entity = await _db.Products
                        .SingleOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"product with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Crafting:
                {
                    var entity = await _db.Craftings
                        .SingleOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"crafting with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Alcohol:
                {
                    var entity = await _db.Alcohols
                        .SingleOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"alcohol with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Drink:
                {
                    var entity = await _db.Drinks
                        .SingleOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"drink with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Crop:
                {
                    var entity = await _db.Crops
                        .SingleOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"crop with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Food:
                {
                    var entity = await _db.Foods
                        .SingleOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"food with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Seafood:
                {
                    var entity = await _db.Seafoods
                        .SingleOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"seafood with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
