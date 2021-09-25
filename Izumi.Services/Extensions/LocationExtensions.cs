using System;
using Izumi.Data.Enums;
using static Izumi.Services.Extensions.ExceptionExtensions;

namespace Izumi.Services.Extensions
{
    public static class LocationExtensions
    {
        public static void CheckRequiredLocation(this LocationType userLocation, LocationType requiredLocation)
        {
            if (userLocation != requiredLocation)
                throw new GameUserExpectedException(userLocation switch
                {
                    LocationType.InTransit =>
                        "сейчас ты находишься в пути и не можешь сделать этого, сперва дождись прибытия в точку назначения.",

                    LocationType.Capital =>
                        $"это действие доступно лишь в **{requiredLocation.Localize(true)}**, напиши `/отправиться` и выбери соответствующую локацию.",

                    LocationType.Garden =>
                        $"это действие доступно лишь в **{requiredLocation.Localize(true)}**, напиши `/отправиться` и выбери соответствующую локацию.",

                    LocationType.Seaport =>
                        $"это действие доступно лишь в **{requiredLocation.Localize(true)}**, напиши `/отправиться` и выбери соответствующую локацию.",

                    LocationType.Castle =>
                        $"это действие доступно лишь в **{requiredLocation.Localize(true)}**, напиши `/отправиться` и выбери соответствующую локацию.",

                    LocationType.Village =>
                        $"это действие доступно лишь в **{requiredLocation.Localize(true)}**, напиши `/отправиться` и выбери соответствующую локацию.",

                    LocationType.ExploreGarden =>
                        "сперва необходимо закончить исследование сада, затем будешь волен делать что угодно.",

                    LocationType.ExploreCastle =>
                        "сперва необходимо закончить исследование шахт, затем будешь волен делать что угодно.",

                    LocationType.Fishing =>
                        "сперва необходимо закончить рыбалку, затем будешь волен делать что угодно.",

                    LocationType.FieldWatering =>
                        "сперва необходимо закончить поливку участка земли, затем будешь волен делать что угодно",

                    LocationType.WorkOnContract =>
                        "сейчас ты работаешь над рабочим контрактом, придется сперва закончить его, таковы правила.",

                    LocationType.CraftingResource =>
                        "изготовление предметов требует полной концентрации, ты не можешь просто бросить все и пойти по своим делам.",

                    LocationType.CraftingAlcohol =>
                        "изготовление алкоголя требует полной концентрации, ты не можешь просто бросить все и пойти по своим делам.",

                    LocationType.CraftingDrink =>
                        "изготовление напитков требует полной концентрации, ты не можешь просто бросить все и пойти по своим делам.",

                    LocationType.CraftingFood =>
                        "приготовление блюд требует полной концентрации, ты не можешь просто бросить все и пойти по своим делам.",

                    _ => throw new ArgumentOutOfRangeException(nameof(userLocation), userLocation, null)
                });
        }
    }
}
