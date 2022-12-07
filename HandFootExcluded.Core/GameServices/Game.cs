using HandFootExcluded.Common;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;

namespace HandFootExcluded.Core.GameServices;

public interface IGame
{
    IRounds Rounds { get; }
    IOrderedEnumerable<IOrderedPlayer> OrderedPlayers { get; }
}

internal sealed partial class GameBuilder : BuilderBase<GameBuilder, IGame>, IGameBuilder, IGameBuilderBuild
{
    private sealed class Game : IGame
    {
        public IRounds Rounds { get; }
        public IOrderedEnumerable<IOrderedPlayer> OrderedPlayers { get; }

        public Game(IRounds rounds, IOrderedEnumerable<IOrderedPlayer> orderedPlayers)
        {
            Rounds = rounds;
            OrderedPlayers = orderedPlayers;
        }
    }
}