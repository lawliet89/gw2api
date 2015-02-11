using System;
using System.Collections.Generic;
using System.Linq;

namespace gw2api.Extension
{
    public static class MiscExtensions
    {
        public static IEnumerable<T> Yield<T>(this T obj)
        {
            yield return obj;
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static string ToCamelCase(this string str)
        {
            return str.Split(new[] {"_"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        public static string ToSnakeCase(this string str)
        {
            return
                String.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()))
                    .ToLower();
        }
    }
}
