using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeScanner.Domain.Extensions
{
    public static class StringExtensions
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

        public static bool IsNullOrEmptyOrWhiteSpace(this string str) =>
            string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);

        public static bool IsMinLength(this string str, int minLength) =>
            !str.IsNullOrEmptyOrWhiteSpace() && str.Length >= minLength;

        public static string ReplaceAll(this string str, IEnumerable<string> oldStrs, string newStr)
        {
            foreach (var oldStr in oldStrs)
                str = str.Replace(oldStr, newStr);
            return str;
        }
    }
}
