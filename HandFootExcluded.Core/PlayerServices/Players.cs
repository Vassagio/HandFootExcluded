using System.Collections;
using HandFootExcluded.Common;

namespace HandFootExcluded.Core.PlayerServices;

public interface IPlayers : IEnumerable<IPositionalPlayer>
{
    IPlayers Add(IPositionalPlayer player);
    void AddRange(IEnumerable<IPositionalPlayer> players);
    TPositionalPlayer Find<TPositionalPlayer>() where TPositionalPlayer : class, IPositionalPlayer;
}

internal sealed class Players : IPlayers
{
    private readonly ISet<IPositionalPlayer> _players = new HashSet<IPositionalPlayer>();
    public int Count => _players.Count;
    public bool IsReadOnly => _players.IsReadOnly;

    public Players() { }

    public Players(IPositionalPlayer player, params IPositionalPlayer[] players) : this()
    {
        Add(player);
        AddRange(players);
    }

    public IPlayers Add(IPositionalPlayer player)
    {
        if (player is null or UnknownPlayer) return this;

        _players.Add(player);

        return this;
    }

    public void AddRange(IEnumerable<IPositionalPlayer> players)
    {
        foreach (var player in players.Where(p => p is not UnknownPlayer))
            _players.Add(player);
    }

    public TPositionalPlayer Find<TPositionalPlayer>() where TPositionalPlayer : class, IPositionalPlayer => 
        _players.OfType<TPositionalPlayer>().SingleOrElse(UnknownPlayer.Instance as TPositionalPlayer);

    public IEnumerator<IPositionalPlayer> GetEnumerator() => _players.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}