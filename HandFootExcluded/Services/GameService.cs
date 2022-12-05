namespace HandFootExcluded.Services;

internal interface IGameService
{
    IGame Create(IReadOnlyList<IPlayer> players);
}

internal sealed partial class GameService : IGameService
{
    private static readonly List<(int AmountToOpen, int StartingPlayer, int StartingPartner, int OpposingPlayer, int OpposingPartner)> _roundOrdering = new()
    {
        new(50, 1, 5, 2, 4),
        new(75, 2, 3, 4, 5),
        new(100, 3, 4, 1, 2),
        new(125, 4, 1, 5, 3),
        new(150, 5, 2, 3, 1)
    };

    private readonly IScoringService _scoringService;

    public GameService(IScoringService scoringService) => _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));

    public IGame Create(IReadOnlyList<IPlayer> players)
    {
        var game = new GameService.Game(players.ToList(), _scoringService);

        for (var i = 1; i <= _roundOrdering.Count(); i++)
            game.Add(CreateRound(i, players, _roundOrdering[i - 1]));

        return game;
    }

    private static IRound CreateRound(int index, IReadOnlyList<IPlayer> players, (int AmountToOpen, int StartingPlayer, int StartingPartner, int OpposingPlayer, int OpposingPartner) roundOrder)
    {
        var startingPlayer = players.Single(p => p.Position == roundOrder.StartingPlayer);
        var startingPartner = players.Single(p => p.Position == roundOrder.StartingPartner);
        var opposingPlayer = players.Single(p => p.Position == roundOrder.OpposingPlayer);
        var opposingPartner = players.Single(p => p.Position == roundOrder.OpposingPartner);

        var nonExcludedPlayers = new List<IPlayer> { startingPlayer, startingPartner, opposingPlayer, opposingPartner };
        var excludedPlayer = players.Except(nonExcludedPlayers).Single();

        return new GameService.Round(index, roundOrder.AmountToOpen, startingPlayer, startingPartner, opposingPlayer, opposingPartner, excludedPlayer);
    }
}