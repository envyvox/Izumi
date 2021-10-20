namespace Izumi.Services.Extensions
{
    public static class StringExtensions
    {
        public static readonly string[] WelcomeMessages = {
            "{0} приединяется к вашей пати.",
            "{0} приземляется на сервере.",
            "Ура, {0} теперь с нами!",
            "{0} запрыгивает на сервер.",
            "Добро пожаловать, {0}. Надеемся, ты к нам не без пиццы!",
            "Дикий {0} появился.",
            "Рады встрече, {0}.",
            "{0} уже с нами!",
            "Рады тебя видеть, {0}",
            "Привет, {0}. Поздоровайся со всеми!",
            "{0} проскальзывает на сервер.",
            "Знакомьтесь, это {0}!",
            "{0} уже здесь."
        };

        /// <summary> Unicode Character “⠀” (U+2800) </summary>
        public const string EmptyChar = "⠀";

        public static string RemoveFromEnd(this string source, int amount)
        {
            return source.Remove(source.Length - amount);
        }
    }
}
