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

internal sealed record RoundOrder(int Order, int OpenAmount, int StartingPlayer, int StartingPartner, int OpposingPlayer, int OpposingPartner) : IRoundOrder;

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
    private IEnumerable<INonPositionalPlayer> _nonPositionalPlayers;
    private IEnumerable<IRoundOrder> _roundOrdering;

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

        var orderedPlayers = Shuffle(_nonPositionalPlayers).ToList();
        var rounds = _roundBuilder.WithOrderedPlayers(orderedPlayers)
                                  .WithRoundOrdering(_roundOrdering)
                                  .Build();

        return new Game(rounds, orderedPlayers);
    }

    private IOrderedEnumerable<IOrderedPlayer> Shuffle(IEnumerable<INonPositionalPlayer> unshuffledPlayers)
    {
        var shuffledPlayers = unshuffledPlayers.ToList()
                                               .Shuffle();
        var orderedPlayers = new List<IOrderedPlayer>();
        for (var i = 1; i <= shuffledPlayers.Count; i++)
            orderedPlayers.Add(_orderedPlayerBuilder.WithPlayer(shuffledPlayers[i - 1])
                                                    .WithOrder(i)
                                                    .Build());

        return orderedPlayers.OrderBy(p => p.Order);
    }
}