using System.Collections;

namespace HandFootExcluded;

internal interface IPlayers : IList<IPlayer> { }

internal sealed class Players : IPlayers
{
    private readonly List<IPlayer> _players = new();
    public int Count => _players.Count;
    public bool IsReadOnly => false;
    public IPlayer this[int index] { get => _players[index]; set => _players[index] = value; }

    public IEnumerator<IPlayer> GetEnumerator() => _players.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(IPlayer player)
    {
        if (player is UnknownPlayer) return;

        _players.Add(player);
    }

    public void Clear() => _players.Clear();
    public bool Contains(IPlayer player) => _players.Contains(player);
    public void CopyTo(IPlayer[] array, int arrayIndex) => _players.CopyTo(array, arrayIndex);
    public bool Remove(IPlayer player) => _players.Remove(player);
    public int IndexOf(IPlayer player) => _players.IndexOf(player);
    public void Insert(int index, IPlayer player) => _players.Insert(index, player);
    public void RemoveAt(int index) => _players.RemoveAt(index);
}