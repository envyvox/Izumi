using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Alcohol.Queries;
using Izumi.Services.Game.Crafting.Queries;
using Izumi.Services.Game.Crop.Queries;
using Izumi.Services.Game.Drink.Queries;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Gathering.Queries;
using Izumi.Services.Game.Ingredient.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Product.Queries;
using Izumi.Services.Game.Seafood.Queries;
using MediatR;
using static Izumi.Services.Extensions.ExceptionExtensions;

namespace Izumi.Services.Game.Ingredient.Commands
{
    public record CheckIngredientAmountInUserCommand(
            long UserId,
            IngredientCategoryType Category,
            Guid IngredientId,
            uint IngredientAmount,
            uint CraftingAmount)
        : IRequest;

    public class CheckIngredientAmountInUserHandler : IRequestHandler<CheckIngredientAmountInUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CheckIngredientAmountInUserHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(CheckIngredientAmountInUserCommand request, CancellationToken ct)
        {
            uint userAmount;

            switch (request.Category)
            {
                case IngredientCategoryType.Gathering:

                    var userGathering = await _mediator.Send(new GetUserGatheringQuery(
                        request.UserId, request.IngredientId));
                    userAmount = userGathering.Amount;

                    break;
                case IngredientCategoryType.Product:

                    var userProduct = await _mediator.Send(new GetUserProductQuery(
                        request.UserId, request.IngredientId));
                    userAmount = userProduct.Amount;

                    break;
                case IngredientCategoryType.Crafting:

                    var userCrafting = await _mediator.Send(new GetUserCraftingQuery(
                        request.UserId, request.IngredientId));
                    userAmount = userCrafting.Amount;

                    break;
                case IngredientCategoryType.Alcohol:

                    var userAlcohol = await _mediator.Send(new GetUserAlcoholQuery(
                        request.UserId, request.IngredientId));
                    userAmount = userAlcohol.Amount;

                    break;
                case IngredientCategoryType.Drink:

                    var userDrink = await _mediator.Send(new GetUserDrinkQuery(
                        request.UserId, request.IngredientId));
                    userAmount = userDrink.Amount;

                    break;
                case IngredientCategoryType.Crop:

                    var userCrop = await _mediator.Send(new GetUserCropQuery(
                        request.UserId, request.IngredientId));
                    userAmount = userCrop.Amount;

                    break;
                case IngredientCategoryType.Food:

                    var userFood = await _mediator.Send(new GetUserFoodQuery(
                        request.UserId, request.IngredientId));
                    userAmount = userFood.Amount;

                    break;
                case IngredientCategoryType.Seafood:

                    var userSeafood = await _mediator.Send(new GetUserSeafoodQuery(
                        request.UserId, request.IngredientId));
                    userAmount = userSeafood.Amount;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (userAmount < request.IngredientAmount * request.CraftingAmount)
            {
                var emotes = DiscordRepository.Emotes;
                var ingredient = await _mediator.Send(new GetIngredientByIdQuery(
                    request.Category, request.IngredientId));

                throw new GameUserExpectedException(
                    "сверяясь со списком ингредиентов, я заметила что в твоем инвентаре недостаточно " +
                    $"{emotes.GetEmote(ingredient.Name)} {_local.Localize(request.Category, ingredient.Name)}.");
            }

            return Unit.Value;
        }
    }
}