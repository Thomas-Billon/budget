using System.Diagnostics.CodeAnalysis;

namespace Budget.Server.Core.Shared
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? str)
        {
            return !str.IsNullOrEmpty();
        }

        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return char.ToLower(str[0]) + str[1..];
        }
    }
}
