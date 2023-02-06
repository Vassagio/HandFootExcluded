using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.GameServices;

public interface IGameService
{
    IGame CreateGame(IEnumerable<string> playerNames, IEnumerable<int> roundOpeningAmounts);
}

internal sealed class GameService : IGameService
{
    private readonly IGameBuilder _gameBuilder;
    private readonly INonPositionalPlayerBuilder _nonPositionalPlayerBuilder;

    public GameService(IGameBuilder gameBuilder, INonPositionalPlayerBuilder nonPositionalPlayerBuilder)
    {
        _gameBuilder = gameBuilder ?? throw new ArgumentNullException(nameof(gameBuilder));
        _nonPositionalPlayerBuilder = nonPositionalPlayerBuilder ?? throw new ArgumentNullException(nameof(nonPositionalPlayerBuilder));
    }

    public IGame CreateGame(IEnumerable<string> playerNames, IEnumerable<int> roundOpeningAmounts)
    {
        var players = CreatePlayers(playerNames);
        var roundOrdering = CreateRoundOrdering(roundOpeningAmounts.ToList());

        return _gameBuilder.WithPlayers(players).WithRoundOrdering(roundOrdering).Build();
    }

    private IEnumerable<INonPositionalPlayer> CreatePlayers(IEnumerable<string> playerNames) => 
        playerNames.Select(playerName => _nonPositionalPlayerBuilder.WithFullName(playerName).Build()).ToList();

    private static IEnumerable<IRoundOrder> CreateRoundOrdering(IReadOnlyList<int> roundOpeningAmounts)
    {
        yield return new RoundOrder(1, roundOpeningAmounts[0], 1, 5, 2, 4);
        yield return new RoundOrder(2, roundOpeningAmounts[1], 2, 3, 4, 5);
        yield return new RoundOrder(3, roundOpeningAmounts[2], 3, 4, 1, 2);
        yield return new RoundOrder(4, roundOpeningAmounts[3], 4, 1, 5, 3);
        yield return new RoundOrder(5, roundOpeningAmounts[4], 5, 2, 3, 1);
    }
}