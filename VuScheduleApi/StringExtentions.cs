using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VuScheduleApi
{
    public static class StringExtentions
    {
        public static string GetStringAfter(this string source, string key)
        {
            int pFrom = source.IndexOf(key) + key.Length;

            return source.Substring(pFrom);
        }

        public static string GetStringFrom(this string source, string key)
        {
            int pFrom = source.IndexOf(key);

            return source.Substring(pFrom);
        }

        public static string GetLastStringAfter(this string source, string key)
        {
            int pFrom = source.LastIndexOf(key) + 1;

            return source.Substring(pFrom);
        }

        public static string GetStringBefore(this string source, string key)
        {
            int pTo = source.LastIndexOf(key);

            return source.Substring(0, pTo);
        }

        public static string GetFirstStringBefore(this string source, string key)
        {
            int pTo = source.IndexOf(key);

            return source.Substring(0, pTo);
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
