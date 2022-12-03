using HandFootExcluded;

namespace HandFootExcluded;

internal interface IGameService
{
    IGame Create(IReadOnlyList<IPlayer> players);
}

internal sealed partial class GameService : IGameService
{
    private readonly IScoringService _scoringService;

    private static readonly List<(int StartingPlayer, int OpposingPlayer, int StartingPartner, int OpposingPartner)> _roundOrdering = new List<(int, int, int, int)>
    {
        new(1, 5, 2, 4),
        new(2, 3, 4, 5),
        new(3, 4, 1, 2),
        new(4, 1, 5, 3),
        new(5, 2, 3, 1),
    };

    public GameService(IScoringService scoringService) => _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));

    public IGame Create(IReadOnlyList<IPlayer> players)
    {
        var game = new Game(players.ToList(), _scoringService);

        for (var i = 1; i <= _roundOrdering.Count(); i++)
            game.Add(CreateRound(i, players, _roundOrdering[i-1]));

        return game;
    }

    private static IRound CreateRound(int index, IReadOnlyList<IPlayer> players, (int StartingPlayer, int OpposingPlayer, int StartingPartner, int OpposingPartner) roundOrder)
    {
        var startingPlayer = players.Single(p => p.Position == roundOrder.StartingPlayer);
        var startingPartner = players.Single(p => p.Position == roundOrder.StartingPartner);
        var opposingPlayer = players.Single(p => p.Position == roundOrder.OpposingPlayer);
        var opposingPartner = players.Single(p => p.Position == roundOrder.OpposingPartner);

        var nonExcludedPlayers = new List<IPlayer> { startingPlayer, startingPartner, opposingPlayer, opposingPartner };
        var excludedPlayer = players.Except(nonExcludedPlayers).Single();

        return new Round(index, startingPlayer, startingPartner, opposingPlayer, opposingPartner, excludedPlayer);
    }
}