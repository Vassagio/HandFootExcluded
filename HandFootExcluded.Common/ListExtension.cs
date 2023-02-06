namespace HandFootExcluded.Common;

public static class ListExtension
{
    private static Random _random;

    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        _random = new Random(Environment.TickCount);
        var unshuffled = list.ToList();
        var n = unshuffled.Count;
        while (n > 1)
        {
            n--;
            var k = _random.Next(n + 1);
            (unshuffled[k], unshuffled[n]) = (unshuffled[n], unshuffled[k]);
        }
        return unshuffled;
    }
}