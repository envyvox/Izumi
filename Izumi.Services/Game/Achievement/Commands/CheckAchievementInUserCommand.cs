using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.Achievement.Queries;
using Izumi.Services.Game.Alcohol.Queries;
using Izumi.Services.Game.Collection.Queries;
using Izumi.Services.Game.Crop.Queries;
using Izumi.Services.Game.Drink.Queries;
using Izumi.Services.Game.Fish.Queries;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Gathering.Queries;
using Izumi.Services.Game.Statistic.Queries;
using MediatR;

namespace Izumi.Services.Game.Achievement.Commands
{
    public record CheckAchievementInUserCommand(long UserId, AchievementType Type) : IRequest;

    public class CheckAchievementInUserHandler : IRequestHandler<CheckAchievementInUserCommand>
    {
        private readonly IMediator _mediator;

        public CheckAchievementInUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckAchievementInUserCommand request, CancellationToken ct)
        {
            var exist = await _mediator.Send(new CheckUserHasAchievementQuery(request.UserId, request.Type));

            if (exist) return Unit.Value;

            switch (request.Type)
            {
                // one-step achievements
                case AchievementType.FirstMessage:
                case AchievementType.FirstTransit:
                case AchievementType.FirstFish:
                case AchievementType.FirstGatheringResource:
                case AchievementType.FirstPlant:
                case AchievementType.FirstCraftResource:
                case AchievementType.FirstCraftAlcohol:
                case AchievementType.FirstCraftDrink:
                case AchievementType.FirstCook:
                case AchievementType.FirstBet:
                case AchievementType.FirstJackPot:
                case AchievementType.FirstLotteryTicket:
                case AchievementType.FirstMarketDeal:
                case AchievementType.FirstContract:
                case AchievementType.CatchEpicFish:
                case AchievementType.CatchMythicalFish:
                case AchievementType.CatchLegendaryFish:
                    await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));
                    break;

                // statistic achievements
                case AchievementType.Gather40Resources:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.Gathering));

                    if (stat.Amount >= 40)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Gather250Resources:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.Gathering));

                    if (stat.Amount >= 250)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Catch50Fish:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.Fishing));

                    if (stat.Amount >= 50)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Catch300Fish:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.Fishing));

                    if (stat.Amount >= 300)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Plant25Seed:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.SeedPlanted));

                    if (stat.Amount >= 25)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Plant150Seed:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.SeedPlanted));

                    if (stat.Amount >= 150)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Collect50Crop:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.CropHarvested));

                    if (stat.Amount >= 50)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Collect300Crop:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.CropHarvested));

                    if (stat.Amount >= 300)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Cook20Food:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.Cooking));

                    if (stat.Amount >= 20)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Cook130Food:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.Cooking));

                    if (stat.Amount >= 130)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Craft30Resource:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.MakingCrafting));

                    if (stat.Amount >= 30)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Craft250Resource:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.MakingCrafting));

                    if (stat.Amount >= 250)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Craft10Alcohol:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.MakingAlcohol));

                    if (stat.Amount >= 10)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Craft80Alcohol:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.MakingAlcohol));

                    if (stat.Amount >= 80)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Casino33Bet:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.CasinoBet));

                    if (stat.Amount >= 33)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Casino777Bet:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.CasinoBet));

                    if (stat.Amount >= 777)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Casino22LotteryBuy:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.CasinoLotteryBuy));

                    if (stat.Amount >= 22)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Casino99LotteryBuy:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.CasinoLotteryBuy));

                    if (stat.Amount >= 99)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Casino20LotteryGift:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.CasinoLotteryGift));

                    if (stat.Amount >= 20)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Market100Sell:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.MarketSell));

                    if (stat.Amount >= 100)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Market666Sell:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.MarketSell));

                    if (stat.Amount >= 666)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Market50Buy:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.MarketBuy));

                    if (stat.Amount >= 50)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.Market333Buy:
                {
                    var stat = await _mediator.Send(new GetUserStatisticQuery(
                        request.UserId, StatisticType.MarketBuy));

                    if (stat.Amount >= 333)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }

                // collection achievements
                case AchievementType.CompleteCollectionGathering:
                {
                    var collection = await _mediator.Send(new GetUserCollectionsQuery(
                        request.UserId, CollectionType.Gathering));
                    var gatherings = await _mediator.Send(new GetGatheringsQuery());

                    if (collection.Count >= gatherings.Count)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.CompleteCollectionCrafting:
                {
                    var collection = await _mediator.Send(new GetUserCollectionsQuery(
                        request.UserId, CollectionType.Gathering));
                    var gatherings = await _mediator.Send(new GetGatheringsQuery());

                    if (collection.Count >= gatherings.Count)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.CompleteCollectionAlcohol:
                {
                    var collection = await _mediator.Send(new GetUserCollectionsQuery(
                        request.UserId, CollectionType.Gathering));
                    var alcohols = await _mediator.Send(new GetAlcoholsQuery());

                    if (collection.Count >= alcohols.Count)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.CompleteCollectionCrop:
                {
                    var collection = await _mediator.Send(new GetUserCollectionsQuery(
                        request.UserId, CollectionType.Gathering));
                    var crops = await _mediator.Send(new GetCropsQuery());

                    if (collection.Count >= crops.Count)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.CompleteCollectionFish:
                {
                    var collection = await _mediator.Send(new GetUserCollectionsQuery(
                        request.UserId, CollectionType.Gathering));
                    var fishes = await _mediator.Send(new GetFishesQuery());

                    if (collection.Count >= fishes.Count)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.CompleteCollectionFood:
                {
                    var collection = await _mediator.Send(new GetUserCollectionsQuery(
                        request.UserId, CollectionType.Gathering));
                    var foods = await _mediator.Send(new GetFoodsQuery());

                    if (collection.Count >= foods.Count)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                case AchievementType.CompleteCollectionDrink:
                {
                    var collection = await _mediator.Send(new GetUserCollectionsQuery(
                        request.UserId, CollectionType.Gathering));
                    var drinks = await _mediator.Send(new GetDrinksQuery());

                    if (collection.Count >= drinks.Count)
                        await _mediator.Send(new AddAchievementToUserCommand(request.UserId, request.Type));

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Unit.Value;
        }
    }
}
