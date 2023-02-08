using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;

namespace HandFootExcluded.Core.GameServices;

public sealed class UnknownGame : IGame
{
    public static readonly UnknownGame Instance = new();

    private static readonly IReadOnlyList<IOrderedPlayer> Players = Enumerable.Empty<IOrderedPlayer>()
                                                                                      .OrderBy(p => p.Order).ToList();

    private UnknownGame() { }

    public IRounds Rounds => UnknownRounds.Instance;
    public IReadOnlyList<IOrderedPlayer> OrderedPlayers => Players;
}