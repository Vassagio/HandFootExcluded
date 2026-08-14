using HandFootExcluded.Core.GameServices;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;
using HandFootExcluded.Core.TeamServices;
using NSubstitute;

namespace HandFootExcluded.Core.Tests.GameServices;

public class GameServiceTest
{
    [Fact]
    public void Initialize()
    {
        var service = BuildGameService();

        Assert.IsAssignableFrom<IGameService>(service);
    }

    [Fact]
    public void CreateGame()
    {
        var positionalPlayerFactory = new PositionalPlayerFactory();
        var teamBuilder = new TeamBuilder();
        var nonPositionalPlayerBuilder = new NonPositionalPlayerBuilder();
        var orderedPlayerBuilder = new OrderedPlayerBuilder();
        var roundBuilder = new RoundBuilder(positionalPlayerFactory, teamBuilder);
        var gameBuilder = new GameBuilder(orderedPlayerBuilder, roundBuilder);
        var service = BuildGameService(gameBuilder, nonPositionalPlayerBuilder);

        var players = new List<IPlayerSetting>()
        {
            PlayerSetting.Create("Player 1", false),
            PlayerSetting.Create("Player 2", false),
            PlayerSetting.Create("Player 3", false),
            PlayerSetting.Create("Player 4", false),
            PlayerSetting.Create("Player 5", false),
        };

        var roundOpeningAmounts = new List<int>()
        {
            50,
            75,
            100,
            125,
            150,
        };

        var result = service.CreateGame(players, roundOpeningAmounts);

        Assert.IsAssignableFrom<IGame>(result);
    }

    private static GameService BuildGameService(IGameBuilder? gameBuilder = null, INonPositionalPlayerBuilder? playerBuilder = null)
    {
        gameBuilder = gameBuilder ?? Substitute.For<IGameBuilder>();
        playerBuilder = playerBuilder ?? Substitute.For<INonPositionalPlayerBuilder>();
        return new GameService(gameBuilder, playerBuilder);
    }
}