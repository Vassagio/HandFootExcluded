namespace HandFootExcluded.Core.PlayerServices;

public sealed class OrderPlayerComparer : IEqualityComparer<IOrderedPlayer>
{
    public static readonly OrderPlayerComparer Instance = new();

    public bool Equals(IOrderedPlayer x, IOrderedPlayer y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;

        return x.Order == y.Order;
    }

    public int GetHashCode(IOrderedPlayer obj) => obj.Order;

    private OrderPlayerComparer(){ }
}