using Discord;

namespace Izumi.Services.Extensions
{
    public static class EmbedBuilderExtensions
    {
        public static EmbedBuilder AddEmptyField(this EmbedBuilder builder, bool inline)
        {
            return builder.AddField(StringExtensions.EmptyChar, StringExtensions.EmptyChar, inline);
        }
    }
}
