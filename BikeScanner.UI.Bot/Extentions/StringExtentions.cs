using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeScanner.UI.Bot.Extentions
{
    public static class StringExtentions
    {
        public static string[] ToMax64BytesValue(this IEnumerable<string> array)
        {
            return array.Select(ToMax64BytesValue).ToArray();
        }

        public static string ToMax64BytesValue(this string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            if (bytes.Length > 64)
            {
                return Encoding.UTF8.GetString(bytes.Take(64).ToArray());
            }
            return value;
        }
    }
}
