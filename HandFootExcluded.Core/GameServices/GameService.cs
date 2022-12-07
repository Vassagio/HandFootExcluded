using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.GameServices;

public interface IGameService
{
    IGame CreateGame(IEnumerable<string> playerNames);
}

internal sealed class GameService : IGameService
{
    private readonly IGameBuilder _gameBuilder;
    private readonly INonPositionalPlayerBuilder _nonPositionalPlayerBuilder;

    public static readonly List<IRoundOrder> RoundOrdering = new()
    {
        new RoundOrder(1, 50, 1, 5, 2, 4),
        new RoundOrder(2, 75, 2, 3, 4, 5),
        new RoundOrder(3, 100, 3, 4, 1, 2),
        new RoundOrder(4, 125, 4, 1, 5, 3),
        new RoundOrder(5, 150, 5, 2, 3, 1)
    };

    public GameService(IGameBuilder gameBuilder, INonPositionalPlayerBuilder nonPositionalPlayerBuilder)
    {
        _gameBuilder = gameBuilder ?? throw new ArgumentNullException(nameof(gameBuilder));
        _nonPositionalPlayerBuilder = nonPositionalPlayerBuilder ?? throw new ArgumentNullException(nameof(nonPositionalPlayerBuilder));
    }

    public IGame CreateGame(IEnumerable<string> playerNames)
    {
        var players = CreatePlayers(playerNames);

        return _gameBuilder.WithPlayers(players).WithRoundOrdering(RoundOrdering).Build();
    }

    private IEnumerable<INonPositionalPlayer> CreatePlayers(IEnumerable<string> playerNames) => 
        playerNames.Select(playerName => _nonPositionalPlayerBuilder.WithFullName(playerName).Build()).ToList();
}