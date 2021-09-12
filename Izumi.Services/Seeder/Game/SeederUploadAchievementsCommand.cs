using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Achievement.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadAchievementsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadAchievementsHandler
        : IRequestHandler<SeederUploadAchievementsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadAchievementsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadAchievementsCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateAchievementCommand[]
            {
                new(AchievementType.FirstMessage, AchievementCategoryType.FirstSteps, "Написать первое сообщение", AchievementRewardType.Ien, 5),
                new(AchievementType.FirstTransit, AchievementCategoryType.FirstSteps, "Впервые отправиться на транспорте", AchievementRewardType.Ien, 30),
                new(AchievementType.FirstFish, AchievementCategoryType.FirstSteps, "Выловить первую рыбу", AchievementRewardType.Ien, 30),
                new(AchievementType.FirstGatheringResource, AchievementCategoryType.FirstSteps, "Собрать свой первый ресурс", AchievementRewardType.Ien, 30),
                new(AchievementType.FirstPlant, AchievementCategoryType.FirstSteps, "Впервые посадить семена", AchievementRewardType.Ien, 30),
                new(AchievementType.FirstCraftResource, AchievementCategoryType.FirstSteps, "Впервые изготовить ресурс", AchievementRewardType.Ien, 30),
                new(AchievementType.FirstCraftAlcohol, AchievementCategoryType.FirstSteps, "Впервые изготовить алкоголь", AchievementRewardType.Ien, 50),
                new(AchievementType.FirstCraftDrink, AchievementCategoryType.FirstSteps, "Впервые изготовить напиток", AchievementRewardType.Ien, 50),
                new(AchievementType.FirstCook, AchievementCategoryType.FirstSteps, "Впервые приготовить блюдо", AchievementRewardType.Ien, 30),
                new(AchievementType.FirstBet, AchievementCategoryType.FirstSteps, "Впервые поставить монеты в казино", AchievementRewardType.Ien, 30),
                new(AchievementType.FirstJackPot, AchievementCategoryType.FirstSteps, "Впервые сорвать джек-пот в казино", AchievementRewardType.Title, (uint) TitleType.Lucky),
                new(AchievementType.FirstLotteryTicket, AchievementCategoryType.FirstSteps, "Купить свой первый лотерейный билет", AchievementRewardType.Ien, 30),
                new(AchievementType.FirstMarketDeal, AchievementCategoryType.FirstSteps, "Впервые купить или продать предмет на рынке", AchievementRewardType.Ien, 30),
                new(AchievementType.FirstContract, AchievementCategoryType.FirstSteps, "Выполнить свой первый рабочий контракт", AchievementRewardType.Ien, 30),
                new(AchievementType.Gather40Resources, AchievementCategoryType.Gathering, "Собрать 40 ресурсов", AchievementRewardType.Pearl, 10),
                new(AchievementType.Gather250Resources, AchievementCategoryType.Gathering, "Собрать 250 ресурсов", AchievementRewardType.Pearl, 20),
                new(AchievementType.Catch50Fish, AchievementCategoryType.Fishing, "Выловить 50 рыб", AchievementRewardType.Pearl, 10),
                new(AchievementType.Catch300Fish, AchievementCategoryType.Fishing, "Выловить 300 рыб", AchievementRewardType.Pearl, 20),
                new(AchievementType.CatchEpicFish, AchievementCategoryType.Fishing, "Поймать эпическую рыбу", AchievementRewardType.Pearl, 5),
                new(AchievementType.CatchMythicalFish, AchievementCategoryType.Fishing, "Поймать мифическую рыбу", AchievementRewardType.Pearl, 10),
                new(AchievementType.CatchLegendaryFish, AchievementCategoryType.Fishing, "Поймать легендарную рыбу", AchievementRewardType.Pearl, 20),
                new(AchievementType.Plant25Seed, AchievementCategoryType.Harvesting, "Посадить 25 семян", AchievementRewardType.Pearl, 10),
                new(AchievementType.Plant150Seed, AchievementCategoryType.Harvesting, "Посадить 150 семян", AchievementRewardType.Pearl, 20),
                new(AchievementType.Collect50Crop, AchievementCategoryType.Harvesting, "Собрать 50 урожая", AchievementRewardType.Pearl, 10),
                new(AchievementType.Collect300Crop, AchievementCategoryType.Harvesting, "Собрать 300 урожая", AchievementRewardType.Pearl, 20),
                new(AchievementType.Cook20Food, AchievementCategoryType.Cooking, "Приготовить 20 блюд", AchievementRewardType.Pearl, 10),
                new(AchievementType.Cook130Food, AchievementCategoryType.Cooking, "Приготовить 130 блюд", AchievementRewardType.Pearl, 20),
                new(AchievementType.Craft30Resource, AchievementCategoryType.Crafting, "Изготовить 30 ресурсов", AchievementRewardType.Pearl, 10),
                new(AchievementType.Craft250Resource, AchievementCategoryType.Crafting, "Изготовить 250 ресурсов", AchievementRewardType.Pearl, 20),
                new(AchievementType.Craft10Alcohol, AchievementCategoryType.Crafting, "Изготовить 10 алкоголя", AchievementRewardType.Pearl, 10),
                new(AchievementType.Craft80Alcohol, AchievementCategoryType.Crafting, "Изготовить 80 алкоголя", AchievementRewardType.Pearl, 20),
                new(AchievementType.Casino33Bet, AchievementCategoryType.Casino, "Сделать 33 ставки", AchievementRewardType.Pearl, 10),
                new(AchievementType.Casino777Bet, AchievementCategoryType.Casino, "Сделать 777 ставок", AchievementRewardType.Title, (uint) TitleType.KingExcitement),
                new(AchievementType.Casino22LotteryBuy, AchievementCategoryType.Casino, "Купить 22 лотерейных билета", AchievementRewardType.Pearl, 10),
                new(AchievementType.Casino99LotteryBuy, AchievementCategoryType.Casino, "Купить 99 лотерейных билетов", AchievementRewardType.Title, (uint) TitleType.BelievingInLuck),
                new(AchievementType.Casino20LotteryGift, AchievementCategoryType.Casino, "Подарить 20 лотерейных билетов", AchievementRewardType.Pearl, 10),
                new(AchievementType.Market100Sell, AchievementCategoryType.Trading, "Продать на рынке 100 предметов", AchievementRewardType.Pearl, 10),
                new(AchievementType.Market666Sell, AchievementCategoryType.Trading, "Продать на рынке 666 предметов", AchievementRewardType.Pearl, 20),
                new(AchievementType.Market50Buy, AchievementCategoryType.Trading, "Купить на рынке 50 предметов", AchievementRewardType.Pearl, 10),
                new(AchievementType.Market333Buy, AchievementCategoryType.Trading, "Купить на рынке 333 предмета", AchievementRewardType.Pearl, 20),
                new(AchievementType.CompleteCollectionGathering, AchievementCategoryType.Collection, "Собрать полную коллекцию ресурсов", AchievementRewardType.Title, (uint) TitleType.AgileEarner),
                new(AchievementType.CompleteCollectionCrafting, AchievementCategoryType.Collection, "Собрать полную коллекцию изготовленных предметов", AchievementRewardType.Title, (uint) TitleType.Handyman),
                new(AchievementType.CompleteCollectionAlcohol, AchievementCategoryType.Collection, "Собрать полную коллекцию алкоголя", AchievementRewardType.Title, (uint) TitleType.WineSamurai),
                new(AchievementType.CompleteCollectionCrop, AchievementCategoryType.Collection, "Собрать полную коллекцию урожая", AchievementRewardType.Title, (uint) TitleType.StockyFarmer),
                new(AchievementType.CompleteCollectionFish, AchievementCategoryType.Collection, "Собрать полную коллекцию рыбы", AchievementRewardType.Title, (uint) TitleType.SeaPoet),
                new(AchievementType.CompleteCollectionFood, AchievementCategoryType.Collection, "Собрать полную коллекцию блюд", AchievementRewardType.Title, (uint) TitleType.CulinaryIdol),
                new(AchievementType.CompleteCollectionDrink, AchievementCategoryType.Collection, "Собрать полную коллекцию напитков", AchievementRewardType.Title, (uint) TitleType.DrinkCollection)
            };

            foreach (var command in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(command);

                    result.Affected++;
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }
    }
}
