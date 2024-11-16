using System.Buffers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Helpers.Extensions
{
    public static class StringExtensions
    {
        private static readonly SearchValues<char> s_numbers = SearchValues.Create("0123456789");

        public static bool RegexIsMatch(this string input, string pattern)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(pattern, nameof(pattern));

            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            return Regex.IsMatch(input, pattern);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ConvertToBase64(this string input) => Convert.ToBase64String(Encoding.UTF8.GetBytes(input));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ConvertFromBase64(this string input) => Encoding.UTF8.GetString(Convert.FromBase64String(input));

        public static string FormattedExceptionStatus(this string input)
        {
            if (input.Length == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder(input.Length + 1);

            var message = input.ToLower().Replace('_', ' ');

            sb.Append(char.ToUpper(message[0]));

            for (int i = 1; i < message.Length; i++)
            {
                sb.Append(message[i]);
            }

            sb.Append('.');

            return sb.ToString();
        }

        public static bool IsStudent(this string? mail)
        {
            var parts = mail?.Split('@', 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts == null || parts.Length != 2)
            {
                throw new ArgumentException("Incorrect mail");
            }

            if (parts[0].AsSpan().IndexOfAny(s_numbers) == -1)
            {
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ClaimOrDefault(this IEnumerable<Claim> claims, string type, string? defaultValue)
        {
            ArgumentNullException.ThrowIfNull(claims, nameof(claims));

            var claim = claims.FirstOrDefault(c => c.Type == type)?.Value;

            return claim ?? defaultValue ?? string.Empty;
        }
    }
}