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
    public record GetIngredientByIdQuery(IngredientCategoryType Category, Guid Id) : IRequest<IngredientDto>;

    public class GetIngredientByIdHandler : IRequestHandler<GetIngredientByIdQuery, IngredientDto>
    {
        private readonly AppDbContext _db;

        public GetIngredientByIdHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<IngredientDto> Handle(GetIngredientByIdQuery request, CancellationToken ct)
        {
            switch (request.Category)
            {
                case IngredientCategoryType.Gathering:
                {
                    var entity = await _db.Gatherings
                        .SingleOrDefaultAsync(x => x.Id == request.Id);

                    if (entity is null)
                    {
                        throw new Exception($"gathering {request.Id} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }

                case IngredientCategoryType.Product:
                {
                    var entity = await _db.Products
                        .SingleOrDefaultAsync(x => x.Id == request.Id);

                    if (entity is null)
                    {
                        throw new Exception($"product {request.Id} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Crafting:
                {
                    var entity = await _db.Craftings
                        .SingleOrDefaultAsync(x => x.Id == request.Id);

                    if (entity is null)
                    {
                        throw new Exception($"crafting {request.Id} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Alcohol:
                {
                    var entity = await _db.Alcohols
                        .SingleOrDefaultAsync(x => x.Id == request.Id);

                    if (entity is null)
                    {
                        throw new Exception($"alcohol {request.Id} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Drink:
                {
                    var entity = await _db.Drinks
                        .SingleOrDefaultAsync(x => x.Id == request.Id);

                    if (entity is null)
                    {
                        throw new Exception($"drink {request.Id} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Crop:
                {
                    var entity = await _db.Crops
                        .SingleOrDefaultAsync(x => x.Id == request.Id);

                    if (entity is null)
                    {
                        throw new Exception($"crop {request.Id} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Food:
                {
                    var entity = await _db.Foods
                        .SingleOrDefaultAsync(x => x.Id == request.Id);

                    if (entity is null)
                    {
                        throw new Exception($"food {request.Id} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategoryType.Seafood:
                {
                    var entity = await _db.Seafoods
                        .SingleOrDefaultAsync(x => x.Id == request.Id);

                    if (entity is null)
                    {
                        throw new Exception($"saefood {request.Id} not found");
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
