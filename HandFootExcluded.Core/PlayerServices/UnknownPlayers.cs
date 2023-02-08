using System.Collections;

namespace HandFootExcluded.Core.PlayerServices;

public sealed class UnknownPlayers : IPlayers
{
    public static readonly UnknownPlayers Instance = new();

    private UnknownPlayers() { }

    public IEnumerator<IPositionalPlayer> GetEnumerator() =>
        Enumerable.Empty<IPositionalPlayer>()
                  .GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IPlayers Add(IPositionalPlayer player) => this;
    public void AddRange(IEnumerable<IPositionalPlayer> players) { }
    public TPositionalPlayer Find<TPositionalPlayer>() where TPositionalPlayer : class, IPositionalPlayer => UnknownPlayer.Instance as TPositionalPlayer;
}