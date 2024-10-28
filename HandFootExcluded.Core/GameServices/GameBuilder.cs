using HandFootExcluded.Common;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;

namespace HandFootExcluded.Core.GameServices;

public interface IRoundOrder
{
    int Order { get; }
    int OpenAmount { get; }
    int StartingPlayer { get; }
    int StartingPartner { get; }
    int OpposingPlayer { get; }
    int OpposingPartner { get; }
}

public interface IGameBuilder : IBuilder
{
    IGameBuilderRoundOrdering WithPlayers(IEnumerable<INonPositionalPlayer> nonPositionalPlayers);
}

public interface IGameBuilderRoundOrdering : IBuilder
{
    IGameBuilderBuild WithRoundOrdering(IEnumerable<IRoundOrder> roundOrdering);
}

public interface IGameBuilderBuild : IBuilder<IGame>
{
}

internal sealed partial class GameBuilder : BuilderBase<GameBuilder, IGame>, IGameBuilder, IGameBuilderRoundOrdering, IGameBuilderBuild
{
    private readonly IOrderedPlayerBuilder _orderedPlayerBuilder;
    private readonly IRoundBuilder _roundBuilder;

    public GameBuilder(IOrderedPlayerBuilder orderedPlayerBuilder, IRoundBuilder roundBuilder)
    {
        _orderedPlayerBuilder = orderedPlayerBuilder ?? throw new ArgumentNullException(nameof(orderedPlayerBuilder));
        _roundBuilder = roundBuilder ?? throw new ArgumentNullException(nameof(roundBuilder));
    }

    public IGameBuilderRoundOrdering WithPlayers(IEnumerable<INonPositionalPlayer> nonPositionalPlayers) => SetProperty(ref _nonPositionalPlayers, nonPositionalPlayers);

    public IGameBuilderBuild WithRoundOrdering(IEnumerable<IRoundOrder> roundOrdering) => SetProperty(ref _roundOrdering, roundOrdering);

    protected override IGame BuildInternal()
    {
        if (_nonPositionalPlayers == null || !_nonPositionalPlayers.Any()) return UnknownGame.Instance;

        var orderedPlayers = Shuffle(_nonPositionalPlayers);
        var rounds = _roundBuilder.WithOrderedPlayers(orderedPlayers)
                                  .WithRoundOrdering(_roundOrdering)
                                  .Build();

        return new Game(rounds, orderedPlayers);
    }

    private IOrderedEnumerable<IOrderedPlayer> Shuffle(IEnumerable<INonPositionalPlayer> unshuffledPlayers)
    {
        var shuffledPlayers = unshuffledPlayers.ToList().Shuffle();
        var orderedPlayers = new List<IOrderedPlayer>();

        if (TryGetExcludedPlayer(shuffledPlayers.ToList(), out var excludedPlayer))
        {
            shuffledPlayers.Remove(excludedPlayer);
            orderedPlayers.Add(_orderedPlayerBuilder.WithPlayer(excludedPlayer).WithOrder(3).Build());
            orderedPlayers.Add(_orderedPlayerBuilder.WithPlayer(shuffledPlayers[0]).WithOrder(1).Build());
            orderedPlayers.Add(_orderedPlayerBuilder.WithPlayer(shuffledPlayers[1]).WithOrder(2).Build());
            orderedPlayers.Add(_orderedPlayerBuilder.WithPlayer(shuffledPlayers[2]).WithOrder(4).Build());
            orderedPlayers.Add(_orderedPlayerBuilder.WithPlayer(shuffledPlayers[3]).WithOrder(5).Build());
        }
        else
            for (var i = 1; i <= shuffledPlayers.Count; i++)
                orderedPlayers.Add(_orderedPlayerBuilder.WithPlayer(shuffledPlayers[i - 1]).WithOrder(i).Build());

        return orderedPlayers.OrderBy(p => p.Order);
    }

    private static bool TryGetExcludedPlayer(IReadOnlyList<INonPositionalPlayer> shuffledPlayers, out INonPositionalPlayer excludedPlayer)
    {
        excludedPlayer = null;
        if (!shuffledPlayers.Any(sp => sp.IsExcludedFirst)) return false;

        excludedPlayer = shuffledPlayers.First(sp => sp.IsExcludedFirst);
        return true;
    }

    private IEnumerable<INonPositionalPlayer> _nonPositionalPlayers;
    private IEnumerable<IRoundOrder> _roundOrdering;
}



internal sealed record RoundOrder(int Order, int OpenAmount, int StartingPlayer, int StartingPartner, int OpposingPlayer, int OpposingPartner) : IRoundOrder;