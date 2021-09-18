using System.Collections.Generic;
using Izumi.Client;

namespace Izumi.Dashboard.Extensions
{
    public static class EmoteExtensions
    {
        /// <summary> Возвращает id иконки по названию, либо id иконки Blank, если первая не найдена. </summary>
        public static long GetEmoteId(this IDictionary<string, EmoteDto> emotes, string emoteName)
        {
            return emotes.TryGetValue(emoteName, out var value)
                ? value.Id
                : emotes.TryGetValue("Blank", out var blankValue)
                    ? blankValue.Id
                    : 813150566695174204;
        }
    }
}
