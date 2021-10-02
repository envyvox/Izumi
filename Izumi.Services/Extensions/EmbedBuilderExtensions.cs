using System.Globalization;
using Discord;

namespace Izumi.Services.Extensions
{
    public static class EmbedBuilderExtensions
    {
        private const string DefaultEmbedColor = "202225";

        public static EmbedBuilder AddEmptyField(this EmbedBuilder builder, bool inline)
        {
            return builder.AddField(StringExtensions.EmptyChar, StringExtensions.EmptyChar, inline);
        }

        public static EmbedBuilder WithDefaultColor(this EmbedBuilder builder)
        {
            return builder.WithColor(new Color(uint.Parse(DefaultEmbedColor, NumberStyles.HexNumber)));
        }
    }
}
