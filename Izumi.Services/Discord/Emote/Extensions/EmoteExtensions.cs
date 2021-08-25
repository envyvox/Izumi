using System.Collections.Generic;
using Izumi.Services.Discord.Emote.Models;

namespace Izumi.Services.Discord.Emote.Extensions
{
    public static class EmoteExtensions
    {
        /// <summary> Возвращает код иконки по названию, либо код иконки Blank, если первая не найдена. </summary>
        public static string GetEmote(this Dictionary<string, EmoteDto> emotes, string emoteName)
        {
            // Ищем в словаре нужную иконку
            return emotes.TryGetValue(emoteName, out var value)
                // Если такая есть - возвращаем ее код
                ? value.Code
                // Если такой нет - ищем код иконки Blank
                : emotes.TryGetValue("Blank", out var blankValue)
                    // Если такая есть - возвращаем ее код
                    ? blankValue.Code
                    // Если такой нет - вероятнее всего словарь пустой и нужно вернуть статичное значение
                    : "<:Blank:813150566695174204>";
        }
    }
}
