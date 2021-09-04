﻿namespace Izumi.Data.Enums
{
    public enum BoxType : byte
    {
        Capital = 1,
        Garden = 2,
        Seaport = 3,
        Castle = 4,
        Village = 5
    }

    public static class BoxHelper
    {
        public static string EmoteName(this BoxType box) => "Box" + box;
    }
}
