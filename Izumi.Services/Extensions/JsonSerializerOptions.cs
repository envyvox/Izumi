using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Izumi.Services.Extensions
{
    public static class JsonSerializerOptions
    {
        public static System.Text.Json.JsonSerializerOptions CyrillicEncoder()
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };

            return options;
        }
    }
}
