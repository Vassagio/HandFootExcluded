using HandFootExcluded.Core.GameServices;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;
using HandFootExcluded.Core.TeamServices;
using NSubstitute;

namespace HandFootExcluded.Core.Tests.RoundServices;

public class RoundBuilderTest
{

    [Fact]
    public void Initialize()
    {
        var builder = BuildRoundBuilder();

        Assert.IsAssignableFrom<IRoundBuilder>(builder);
    }

    [Fact]
    public void Create()
    {
        var positionalPlayerFactory = new PositionalPlayerFactory();
        var teamBuilder = new TeamBuilder();
        var orderedPlayers = GetOrderedPlayers().ToList();
        var roundOrdering = GetRoundOrdering();

        var builder = BuildRoundBuilder(positionalPlayerFactory, teamBuilder);

        var result = builder.WithOrderedPlayers(orderedPlayers)
                            .WithRoundOrdering(roundOrdering)
                            .Build();

        Assert.IsAssignableFrom<IRounds>(result);
        Assert.Collection(result, 
            item => Assert.Equal(1, item.Order),
            item => Assert.Equal(2, item.Order),
            item => Assert.Equal(3, item.Order),
            item => Assert.Equal(4, item.Order),
            item => Assert.Equal(5, item.Order));

        Assert.Collection(result,
            item => Assert.Equal(50, item.OpenAmount),
            item => Assert.Equal(75, item.OpenAmount),
            item => Assert.Equal(100, item.OpenAmount),
            item => Assert.Equal(125, item.OpenAmount),
            item => Assert.Equal(150, item.OpenAmount));
    }

    private static IOrderedEnumerable<IOrderedPlayer> GetOrderedPlayers()
    {
        var player1 = new OrderedPlayer(1, "Player 1", "Player", "", "1", "P1");
        var player2 = new OrderedPlayer(2, "Player 2", "Player", "", "2", "P2");
        var player3 = new OrderedPlayer(3, "Player 3", "Player", "", "3", "P3");
        var player4 = new OrderedPlayer(4, "Player 4", "Player", "", "4", "P4");
        var player5 = new OrderedPlayer(5, "Player 5", "Player", "", "5", "P5");
        
        return new List<IOrderedPlayer>
        {
            player1,
            player2,
            player3,
            player4,
            player5
        }.OrderBy(p => p.Order);
    }

    private static IEnumerable<IRoundOrder> GetRoundOrdering()
    {
        yield return new RoundOrder(1, 50, 1, 5, 2, 4);
        yield return new RoundOrder(2, 75, 2, 3, 4, 5);
        yield return new RoundOrder(3, 100, 3, 4, 1, 2);
        yield return new RoundOrder(4, 125, 4, 1, 5, 3);
        yield return new RoundOrder(5, 150, 5, 2, 3, 1);
    }

    private static IRoundBuilder BuildRoundBuilder(IPositionalPlayerFactory? positionalPlayerFactory = null, ITeamBuilder? teamBuilder = null)
    {
        positionalPlayerFactory = positionalPlayerFactory ?? Substitute.For<IPositionalPlayerFactory>();
        teamBuilder = teamBuilder ?? Substitute.For<ITeamBuilder>();
        return new RoundBuilder(positionalPlayerFactory, teamBuilder);
    }
}