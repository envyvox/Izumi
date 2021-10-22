namespace Izumi.Services.Extensions
{
    public static class StringExtensions
    {
        /// <summary> Unicode Character “⠀” (U+2800) </summary>
        public const string EmptyChar = "⠀";

        public static string RemoveFromEnd(this string source, int amount)
        {
            return source.Remove(source.Length - amount);
        }
    }
}
