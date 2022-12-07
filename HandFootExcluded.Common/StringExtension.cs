using System.Text.RegularExpressions;

namespace HandFootExcluded.Common;

public static class StringExtension
{
    public static string RemoveMultipleSpaces(this string text) => Regex.Replace(text, @"\s+", " ");
}