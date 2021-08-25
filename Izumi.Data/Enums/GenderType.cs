using System;

namespace Izumi.Data.Enums
{
    public enum GenderType : byte
    {
        None = 0,
        Male = 1,
        Female = 2
    }

    public static class GenderHelper
    {
        public static string Localize(this GenderType gender) => gender switch
        {
            GenderType.None => "Не указан",
            GenderType.Male => "Мужской",
            GenderType.Female => "Женский",
            _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
        };

        public static string EmoteName(this GenderType gender) => "Gender" + gender;
    }
}
