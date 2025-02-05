using System;
using System.Linq;

namespace Firebend.AutoCrud.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Coalesce(this string source, string substitution)
            => string.IsNullOrWhiteSpace(source) ? substitution : source;

        public static T? ParseEnum<T>(this string source)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            if (Enum.TryParse(source, true, out T outEnum))
            {
                return outEnum;
            }

            return null;
        }

        public static bool In(this string source, params string[] list) => list.Any(x => x.EqualsIgnoreCaseAndWhitespace(source));

        public static bool EqualsIgnoreCaseAndWhitespace(this string source, string compare)
        {
            if (source == null && compare == null)
            {
                return true;
            }

            if (source == null || compare == null)
            {
                return false;
            }

            return source.SafeTrim().Equals(compare.SafeTrim(), StringComparison.OrdinalIgnoreCase);
        }

        public static string SafeTrim(this string source) => string.IsNullOrEmpty(source) ? source : source.Trim();

        public static string FirstCharToLower(this string str)
        {
            if (char.IsLower(str[0]))
            {
                return str;
            }

            return string.Create(str.Length, str, (output, input) =>
            {
                input.CopyTo(output);
                output[0] = char.ToLowerInvariant(input[0]);
            });
        }
    }
}
