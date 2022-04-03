using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeScanner.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string ToDecimalStringList(this IEnumerable<string> enumerable)
        {
            var list = new StringBuilder();
            var array = enumerable.ToArray();

            for (int i = 0; i < array.Length; i++)
            {
                list.AppendLine($"{i + 1}. {array[i]}");
            }
            return list.ToString();
        }

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

        public static bool IsNullOrEmptyOrWhiteSpace(this string str) =>
            string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);

        public static bool IsMinLength(this string str, int minLength) =>
            !str.IsNullOrEmptyOrWhiteSpace() && str.Length >= minLength;
    }
}
