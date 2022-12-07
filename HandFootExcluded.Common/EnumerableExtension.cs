namespace HandFootExcluded.Common;

public static class EnumerableExtension
{
    public static TResult FirstOrElse<TType, TResult>(this IEnumerable<TType> items, Func<TResult, bool> predicate, TResult @default) where TResult : TType => 
        items.OfType<TResult>().FirstOrDefault(predicate) ?? @default;

    public static TResult FirstOrElse<TType, TResult>(this IEnumerable<TType> items, TResult @default) where TResult : TType =>
        items.OfType<TResult>().FirstOrDefault() ?? @default;

    public static TResult SingleOrElse<TType, TResult>(this IEnumerable<TType> items, Func<TResult, bool> predicate, TResult @default) where TResult : TType =>
        items.OfType<TResult>().SingleOrDefault(predicate) ?? @default;

    public static TResult SingleOrElse<TType, TResult>(this IEnumerable<TType> items, TResult @default) where TResult : TType =>
        items.OfType<TResult>().SingleOrDefault() ?? @default;
}