using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Game.Alcohol.Commands;
using Izumi.Services.Game.Crafting.Commands;
using Izumi.Services.Game.Crop.Commands;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Drink.Commands;
using Izumi.Services.Game.Food.Commands;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Gathering.Commands;
using Izumi.Services.Game.Product.Commands;
using Izumi.Services.Game.Seafood.Commands;
using Izumi.Services.Game.Tutorial.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Game.Tutorial.Commands
{
    public record CheckUserTutorialStepCommand(long UserId, TutorialStepType Step) : IRequest;

    public class CheckUserTutorialStepHandler : IRequestHandler<CheckUserTutorialStepCommand>
    {
        private readonly IMediator _mediator;

        public CheckUserTutorialStepHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckUserTutorialStepCommand request, CancellationToken ct)
        {
            var userStep = await _mediator.Send(new GetUserTutorialStepQuery(request.UserId));

            if (userStep != request.Step) return Unit.Value;

            var nextStep = (TutorialStepType) request.Step.GetHashCode() + 1;

            await _mediator.Send(new UpdateUserTutorialStepCommand(request.UserId, nextStep));

            switch (nextStep)
            {
                case TutorialStepType.CookFriedEgg:
                {
                    var foodIncId = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.TutorialEatFoodIncId));

                    var food = await _mediator.Send(new GetFoodByIncIdQuery(foodIncId));

                    await _mediator.Send(new CreateUserRecipeCommand(request.UserId, food.Id));

                    foreach (var ingredient in food.Ingredients)
                    {
                        switch (ingredient.Category)
                        {
                            case IngredientCategoryType.Gathering:
                                await _mediator.Send(new AddGatheringToUserCommand(
                                    request.UserId, ingredient.IngredientId, ingredient.Amount));
                                break;
                            case IngredientCategoryType.Product:
                                await _mediator.Send(new AddProductToUserCommand(
                                    request.UserId, ingredient.IngredientId, ingredient.Amount));
                                break;
                            case IngredientCategoryType.Crafting:
                                await _mediator.Send(new AddCraftingToUserCommand(
                                    request.UserId, ingredient.IngredientId, ingredient.Amount));
                                break;
                            case IngredientCategoryType.Alcohol:
                                await _mediator.Send(new AddAlcoholToUserCommand(
                                    request.UserId, ingredient.IngredientId, ingredient.Amount));
                                break;
                            case IngredientCategoryType.Drink:
                                await _mediator.Send(new AddDrinkToUserCommand(
                                    request.UserId, ingredient.IngredientId, ingredient.Amount));
                                break;
                            case IngredientCategoryType.Crop:
                                await _mediator.Send(new AddCropToUserCommand(
                                    request.UserId, ingredient.IngredientId, ingredient.Amount));
                                break;
                            case IngredientCategoryType.Food:
                                await _mediator.Send(new AddFoodToUserCommand(
                                    request.UserId, ingredient.IngredientId, ingredient.Amount));
                                break;
                            case IngredientCategoryType.Seafood:
                                await _mediator.Send(new AddSeafoodToUserCommand(
                                    request.UserId, ingredient.IngredientId, ingredient.Amount));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    break;
                }

                case TutorialStepType.TransitToCastle:
                {
                    var foodIncId = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.TutorialSpecialFoodIncId));
                    var amount = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.TutorialSpecialFoodAmount));

                    var food = await _mediator.Send(new GetFoodByIncIdQuery(foodIncId));

                    await _mediator.Send(new AddFoodToUserCommand(request.UserId, food.Id, amount));

                    break;
                }

                case TutorialStepType.Completed:

                    var currencyAmount = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.EconomyTutorialReward));

                    await _mediator.Send(new AddCurrencyToUserCommand(request.UserId, CurrencyType.Ien,
                        currencyAmount));

                    break;
            }

            var embed = new EmbedBuilder()
                .WithAuthor(nextStep.Name())
                .WithDescription(nextStep.Description());

            return await _mediator.Send(new SendEmbedToUserCommand((ulong) request.UserId, embed));
        }
    }
}