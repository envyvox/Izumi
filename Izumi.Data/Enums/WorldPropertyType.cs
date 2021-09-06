﻿namespace Izumi.Data.Enums
{
    public enum WorldPropertyType : byte
    {
        EnergyCostExplore = 1,
        EnergyCostTransit = 2,
        EnergyCostMaking= 3,
        EnergyCostFieldPlant = 4,
        EnergyCostFieldCollect = 5,
        EnergyCostFieldWater = 6,
        EnergyCostFieldDig = 7,
        EnergyRecoveryNonPremium = 8,
        EnergyRecoveryPremium = 9,
        ActionTimeMinutesFieldWater = 10,
        ActionTimeMinutesExplore = 11,
        EconomyStartup = 12,
        EconomyDailyIncome = 13,
        EconomyTutorialReward = 14,
        EconomyFoodEnergyPrice = 15,
        OpenBoxCapitalMinAmount = 16,
        OpenBoxCapitalMaxAmount = 17,
        OpenBoxSeaportMinAmount = 18,
        OpenBoxSeaportMaxAmount = 19,
        OpenBoxSeaportRarity = 20,
        OpenBoxVillageProductMinAmount = 21,
        OpenBoxVillageProductMaxAmount = 22,
        OpenBoxVillageCropMinAmount = 23,
        OpenBoxVillageCropMaxAmount = 24,
        ActionTimePremiumReduceTransitPercent = 25
    }
}
