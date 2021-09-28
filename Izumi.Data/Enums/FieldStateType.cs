using System;

namespace Izumi.Data.Enums
{
    public enum FieldStateType : byte
    {
        Empty = 0,
        Planted = 1,
        Watered = 2,
        Completed = 3
    }

    public static class FieldStateHelper
    {
        public static string Localize(this FieldStateType state) => state switch
        {
            FieldStateType.Empty => "Пустая",
            FieldStateType.Planted => "Засажена",
            FieldStateType.Watered => "Полита",
            FieldStateType.Completed => "Готово к сбору",
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}
