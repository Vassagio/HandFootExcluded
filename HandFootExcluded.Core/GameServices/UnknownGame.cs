using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;

namespace HandFootExcluded.Core.GameServices;

public sealed class UnknownGame : IGame
{
    public static readonly UnknownGame Instance = new();
    private static readonly IOrderedEnumerable<IOrderedPlayer> _orderedPlayers = Enumerable.Empty<IOrderedPlayer>().OrderBy(p => p.Order);

    public IRounds Rounds => UnknownRounds.Instance;
    public IOrderedEnumerable<IOrderedPlayer> OrderedPlayers => _orderedPlayers;

    private UnknownGame() {}
}