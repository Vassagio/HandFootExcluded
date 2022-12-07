using System.Collections;

namespace HandFootExcluded.Core.PlayerServices;

public interface IPlayers : ISet<IPositionalPlayer> { }

internal sealed class Players : IPlayers
{
    private readonly ISet<IPositionalPlayer> _players = new HashSet<IPositionalPlayer>();
    public int Count => _players.Count;
    public bool IsReadOnly => _players.IsReadOnly;

    public IEnumerator<IPositionalPlayer> GetEnumerator() => _players.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    void ICollection<IPositionalPlayer>.Add(IPositionalPlayer player)
    {
        if (player is null or UnknownPlayer) return;

        _players.Add(player);
    }

    public void ExceptWith(IEnumerable<IPositionalPlayer> players) => _players.ExceptWith(players);
    public void IntersectWith(IEnumerable<IPositionalPlayer> players) => _players.IntersectWith(players);
    public bool IsProperSubsetOf(IEnumerable<IPositionalPlayer> players) => _players.IsProperSubsetOf(players);
    public bool IsProperSupersetOf(IEnumerable<IPositionalPlayer> players) => _players.IsProperSupersetOf(players);
    public bool IsSubsetOf(IEnumerable<IPositionalPlayer> players) => _players.IsSubsetOf(players);
    public bool IsSupersetOf(IEnumerable<IPositionalPlayer> players) => _players.IsSupersetOf(players);
    public bool Overlaps(IEnumerable<IPositionalPlayer> players) => _players.Overlaps(players);
    public bool SetEquals(IEnumerable<IPositionalPlayer> players) => _players.SetEquals(players);
    public void SymmetricExceptWith(IEnumerable<IPositionalPlayer> players) => _players.SymmetricExceptWith(players);
    public void UnionWith(IEnumerable<IPositionalPlayer> players) => _players.UnionWith(players);
    bool ISet<IPositionalPlayer>.Add(IPositionalPlayer player) => player is not UnknownPlayer && _players.Add(player);
    public void Clear() => _players.Clear();
    public bool Contains(IPositionalPlayer player) => _players.Contains(player);
    public void CopyTo(IPositionalPlayer[] array, int arrayIndex) => _players.CopyTo(array, arrayIndex);
    public bool Remove(IPositionalPlayer player) => _players.Remove(player);
}