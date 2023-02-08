using HandFootExcluded.Common;
using HandFootExcluded.Core.GameServices;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.TeamServices;

namespace HandFootExcluded.Core.RoundServices;

public interface IRoundBuilder
{
    IRoundBuilderRounds WithOrderedPlayers(IReadOnlyList<IOrderedPlayer> orderedPlayers);
}

public interface IRoundBuilderRounds
{
    IRoundBuilderBuild WithRoundOrdering(IEnumerable<IRoundOrder> roundOrdering);
}

public interface IRoundBuilderBuild : IBuilder<IRounds>
{
}

public sealed class RoundBuilder : BuilderBase<RoundBuilder, IRounds>, IRoundBuilder, IRoundBuilderRounds, IRoundBuilderBuild
{
    private readonly IPositionalPlayerFactory _positionalPlayerFactory;
    private readonly ITeamBuilder _teamBuilder;
    private IReadOnlyList<IOrderedPlayer> _orderedPlayers;
    private IEnumerable<IRoundOrder> _roundOrdering;

    public RoundBuilder(IPositionalPlayerFactory positionalPlayerFactory, ITeamBuilder teamBuilder)
    {
        _positionalPlayerFactory = positionalPlayerFactory ?? throw new ArgumentNullException(nameof(positionalPlayerFactory));
        _teamBuilder = teamBuilder ?? throw new ArgumentNullException(nameof(teamBuilder));
    }

    public IRoundBuilderRounds WithOrderedPlayers(IReadOnlyList<IOrderedPlayer> orderedPlayers) => SetProperty(ref _orderedPlayers, orderedPlayers);
    public IRoundBuilderBuild WithRoundOrdering(IEnumerable<IRoundOrder> roundOrdering) => SetProperty(ref _roundOrdering, roundOrdering);

    protected override IRounds BuildInternal()
    {
        var rounds = new Rounds();

        if (_orderedPlayers is null || !_orderedPlayers.Any()) return rounds;
        if (_roundOrdering is null || !_roundOrdering.Any()) return rounds;

        foreach (var roundOrder in _roundOrdering)
        {
            var positionalPlayers = GetPositionalPlayers(roundOrder);
            if (positionalPlayers.Any(p => p is UnknownPlayer)) return rounds;

            var teams = GetTeams(positionalPlayers);

            var round = new Round(roundOrder.Order, roundOrder.OpenAmount, positionalPlayers, teams);
            rounds.Add(round);
        }

        return rounds;
    }

    private IPlayers GetPositionalPlayers(IRoundOrder roundOrder)
    {
        var positionalPlayers = new Players
        {
            FindPlayer<IStartingPlayer>(_orderedPlayers, roundOrder.StartingPlayer),
            FindPlayer<IStartingPartner>(_orderedPlayers, roundOrder.StartingPartner),
            FindPlayer<IOpposingPlayer>(_orderedPlayers, roundOrder.OpposingPlayer),
            FindPlayer<IOpposingPartner>(_orderedPlayers, roundOrder.OpposingPartner)
        };

        return positionalPlayers.Add(FindExcludedPlayer(_orderedPlayers, positionalPlayers));
    }

    private TPositionalPlayer FindPlayer<TPositionalPlayer>(IEnumerable<IOrderedPlayer> orderedPlayers, int playerOrder) where TPositionalPlayer : class, IPositionalPlayer
    {
        var player = orderedPlayers.FirstOrDefault(p => p.Order == playerOrder) ?? UnknownPlayer.Instance;
        return _positionalPlayerFactory.Create<TPositionalPlayer>(player) as TPositionalPlayer;
    }

    private IExcludedPlayer FindExcludedPlayer(IEnumerable<IOrderedPlayer> orderedPlayers, IPlayers positionalPlayers)
    {
        var excludedPlayer = orderedPlayers.OfType<OrderedPlayer>()
                                           .Except(positionalPlayers.OfType<OrderedPlayer>(), OrderPlayerComparer.Instance)
                                           .FirstOrDefault();
        return _positionalPlayerFactory.Create<IExcludedPlayer>(excludedPlayer) as IExcludedPlayer;
    }

    private ITeams GetTeams(IPlayers players) =>
        new Teams
        {
            _teamBuilder.WithPlayer(players.Find<IStartingPlayer>())
                        .WithPartner(players.Find<IStartingPartner>())
                        .Build(),
            _teamBuilder.WithPlayer(players.Find<IOpposingPlayer>())
                        .WithPartner(players.Find<IOpposingPartner>())
                        .Build()
        };
}