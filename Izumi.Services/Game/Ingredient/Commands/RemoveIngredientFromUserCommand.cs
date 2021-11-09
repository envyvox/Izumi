using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.Alcohol.Commands;
using Izumi.Services.Game.Crafting.Commands;
using Izumi.Services.Game.Crop.Commands;
using Izumi.Services.Game.Drink.Commands;
using Izumi.Services.Game.Food.Commands;
using Izumi.Services.Game.Gathering.Commands;
using Izumi.Services.Game.Product.Commands;
using Izumi.Services.Game.Seafood.Commands;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Commands
{
    public record RemoveIngredientFromUserCommand(
            long UserId,
            IngredientCategoryType Category,
            Guid IngredientId,
            uint IngredientAmount,
            uint CraftingAmount)
        : IRequest;

    public class RemoveIngredientFromUserHandler : IRequestHandler<RemoveIngredientFromUserCommand>
    {
        private readonly IMediator _mediator;

        public RemoveIngredientFromUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveIngredientFromUserCommand request, CancellationToken ct)
        {
            switch (request.Category)
            {
                case IngredientCategoryType.Gathering:

                    return await _mediator.Send(new RemoveGatheringFromUserCommand(
                        request.UserId, request.IngredientId, request.IngredientAmount * request.CraftingAmount));

                case IngredientCategoryType.Product:

                    return await _mediator.Send(new RemoveProductFromUserCommand(
                        request.UserId, request.IngredientId, request.IngredientAmount * request.CraftingAmount));

                case IngredientCategoryType.Crafting:

                    return await _mediator.Send(new RemoveCraftingFromUserCommand(
                        request.UserId, request.IngredientId, request.IngredientAmount * request.CraftingAmount));

                case IngredientCategoryType.Alcohol:

                    return await _mediator.Send(new RemoveAlcoholFromUserCommand(
                        request.UserId, request.IngredientId, request.IngredientAmount * request.CraftingAmount));

                case IngredientCategoryType.Drink:

                    return await _mediator.Send(new RemoveDrinkFromUserCommand(
                        request.UserId, request.IngredientId, request.IngredientAmount * request.CraftingAmount));

                case IngredientCategoryType.Crop:

                    return await _mediator.Send(new RemoveCropFromUserCommand(
                        request.UserId, request.IngredientId, request.IngredientAmount * request.CraftingAmount));

                case IngredientCategoryType.Food:

                    return await _mediator.Send(new RemoveFoodFromUserCommand(
                        request.UserId, request.IngredientId, request.IngredientAmount * request.CraftingAmount));

                case IngredientCategoryType.Seafood:

                    return await _mediator.Send(new RemoveSeafoodFromUserCommand(
                        request.UserId, request.IngredientId, request.IngredientAmount * request.CraftingAmount));

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}