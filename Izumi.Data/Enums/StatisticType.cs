using System;

namespace Izumi.Data.Enums
{
    public enum StatisticType : byte
    {
        Messages = 1,
        VoiceMinutes = 2,
        Transit = 3,
        Gathering = 4,
        Fishing = 5,
        SeedPlanted = 6,
        CropHarvested = 7,
        MakingCrafting = 8,
        MakingAlcohol = 9,
        MakingDrink = 10,
        Cooking = 11,
        CasinoBet = 12,
        CasinoJackpot = 13,
        CasinoLotteryBuy = 14,
        CasinoLotteryGift = 15,
        CasinoLotteryWin = 16,
        MarketSell = 17,
        MarketBuy = 18,
        Contracts = 19,
        BossKilled = 20
    }

    public static class StatisticHelper
    {
        public static string Localize(this StatisticType statistic) => statistic switch
        {
            StatisticType.Messages => "сообщений",
            StatisticType.VoiceMinutes => "голосовая активность",
            StatisticType.Transit => "Перемещений",
            StatisticType.Gathering => "Добыто предметов",
            StatisticType.Fishing => "Выловлено рыб",
            StatisticType.SeedPlanted => "Посажено семян",
            StatisticType.CropHarvested => "Собрано урожая",
            StatisticType.MakingCrafting => "Изготовлено предметов",
            StatisticType.MakingAlcohol => "Изготовлено алкоголя",
            StatisticType.MakingDrink => "Изготовлено напитков",
            StatisticType.Cooking => "Приготовлено блюд",
            StatisticType.CasinoBet => "Сделано ставок",
            StatisticType.CasinoJackpot => "Джек-потов",
            StatisticType.CasinoLotteryBuy => "Куплено лотерейных билетов",
            StatisticType.CasinoLotteryGift => "Подарено лотерейных билетов",
            StatisticType.CasinoLotteryWin => "Побед в лотерее",
            StatisticType.MarketSell => "Продано предметов на рынке",
            StatisticType.MarketBuy => "Куплено предметов на рынке",
            StatisticType.Contracts => "Выполнено контрактов",
            StatisticType.BossKilled => "Убито ежедневных боссов",
            _ => throw new ArgumentOutOfRangeException(nameof(statistic), statistic, null)
        };
    }
}
