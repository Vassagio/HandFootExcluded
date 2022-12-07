namespace HandFootExcluded.Common;

public static class ListExtension
{
    private static readonly Random RANDOM = new();

    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        var unshuffled = list.ToList();
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = RANDOM.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
        return unshuffled;
    }
}