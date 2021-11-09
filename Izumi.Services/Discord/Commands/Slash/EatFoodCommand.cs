using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Food.Commands;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Localization.Queries;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash
{
    public record EatFoodCommand(SocketSlashCommand Command) : IRequest;

    public class EatFoodHandler : IRequestHandler<EatFoodCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public EatFoodHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(EatFoodCommand request, CancellationToken ct)
        {
            var amount = (uint) (long) request.Command.Data.Options.First().Value;
            var foodName = (string) request.Command.Data.Options.Last().Value;

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var foodLocal = await _mediator.Send(new GetLocalizationByLocalizedNameQuery(
                LocalizationCategoryType.Food, foodName));
            var food = await _mediator.Send(new GetFoodByNameQuery(foodLocal.Name));
            var userFood = await _mediator.Send(new GetUserFoodQuery(user.Id, food.Id));

            var embed = new EmbedBuilder();

            if (userFood.Amount < amount)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя нет столько {emotes.GetEmote(food.Name)} {_local.Localize(LocalizationCategoryType.Food, food.Name)} " +
                    "сколько ты хочешь съесть.");
            }
            else
            {
                var costPrice = await _mediator.Send(new GetFoodCostPriceQuery(food.Ingredients));
                var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice, 1));
                var foodEnergy = await _mediator.Send(new GetFoodEnergyRechargeQuery(costPrice, craftingPrice));

                await _mediator.Send(new RemoveFoodFromUserCommand(user.Id, food.Id, amount));
                await _mediator.Send(new AddEnergyToUserCommand(user.Id, foodEnergy * amount));

                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"ты съел {emotes.GetEmote(food.Name)} {amount} " +
                    $"{_local.Localize(LocalizationCategoryType.Food, food.Name, amount)} и получил " +
                    $"{emotes.GetEmote("Energy")} {foodEnergy * amount} " +
                    $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", foodEnergy * amount)}.");

                var tutorialEatFoodIncId = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.TutorialEatFoodIncId));

                if (food.AutoIncrementedId == tutorialEatFoodIncId)
                    await _mediator.Send(new CheckUserTutorialStepCommand(user.Id, TutorialStepType.EatFriedEgg));
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}