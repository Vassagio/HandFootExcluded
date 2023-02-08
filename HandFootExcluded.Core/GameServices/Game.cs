using HandFootExcluded.Common;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;

namespace HandFootExcluded.Core.GameServices;

public interface IGame
{
    IRounds Rounds { get; }
    IReadOnlyList<IOrderedPlayer> OrderedPlayers { get; }
}

internal sealed partial class GameBuilder : BuilderBase<GameBuilder, IGame>, IGameBuilder, IGameBuilderBuild
{
    private sealed class Game : IGame
    {
        public Game(IRounds rounds, IReadOnlyList<IOrderedPlayer> orderedPlayers)
        {
            Rounds = rounds;
            OrderedPlayers = orderedPlayers;
        }
        public IRounds Rounds { get; }
        public IReadOnlyList<IOrderedPlayer> OrderedPlayers { get; }
    }
}