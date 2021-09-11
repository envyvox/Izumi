using System;
using System.Collections.Generic;
using System.Linq;

namespace Izumi.Data.Enums
{
    public enum ReputationType : byte
    {
        Capital = 1,
        Garden = 2,
        Seaport = 3,
        Castle = 4,
        Village = 5
    }

    public static class ReputationHelper
    {
        private static readonly Dictionary<uint, uint> ReputationStarsBrackets = new()
        {
            { 0, 0 }, { 500, 1 }, { 1000, 2 }, { 2000, 3 }, { 5000, 4 }, { 10000, 5 }
        };

        public static LocationType Location(this ReputationType reputation) => reputation switch
        {
            ReputationType.Capital => LocationType.Capital,
            ReputationType.Garden => LocationType.Garden,
            ReputationType.Seaport => LocationType.Seaport,
            ReputationType.Castle => LocationType.Castle,
            ReputationType.Village => LocationType.Village,
            _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
        };

        public static string Emote(this ReputationType reputation, long amount) =>
            "Reputation" +
            reputation.Location() +
            ReputationStarsBrackets[ReputationStarsBrackets.Keys.Where(x => x <= amount).Max()] +
            "S";
    }
}
