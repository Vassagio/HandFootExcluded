using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.TeamServices;
using NSubstitute;

namespace HandFootExcluded.Core.Tests.TeamServices;
public class TeamBuilderTest
{
    [Fact]
    public void Initialize()
    {
        var builder = BuildTeamBuilder();

        Assert.IsAssignableFrom<ITeamBuilder>(builder);
    }

    [Fact]
    public void StartingPlayer_StartingPartner_ReturnsStartingTeam()
    {
        var startingPlayer = Substitute.For<IStartingPlayer>();
        var startingPartner = Substitute.For<IStartingPartner>();
        var builder = BuildTeamBuilder();

        var result = builder.WithPlayer(startingPlayer)
                            .WithPartner(startingPartner)
                            .Build();

        Assert.IsAssignableFrom<IStartingTeam>(result);
        Assert.Equal(result.Player, startingPlayer);
        Assert.Equal(result.Partner, startingPartner);
        Assert.Collection(result.Players, 
            item => Assert.Equal(startingPlayer, item),
            item => Assert.Equal(startingPartner, item));
    }

    [Fact]
    public void OpposingPlayer_OpposingPartner_ReturnsOpposingTeam()
    {
        var opposingPlayer = Substitute.For<IOpposingPlayer>();
        var opposingPartner = Substitute.For<IOpposingPartner>();
        var builder = BuildTeamBuilder();

        var result = builder.WithPlayer(opposingPlayer)
                            .WithPartner(opposingPartner)
                            .Build();

        Assert.IsAssignableFrom<IOpposingTeam>(result);
        Assert.Equal(result.Player, opposingPlayer);
        Assert.Equal(result.Partner, opposingPartner);
        Assert.Collection(result.Players,
            item => Assert.Equal(opposingPlayer, item),
            item => Assert.Equal(opposingPartner, item));
    }

    [Fact]
    public void StartingPlayer_OpposingPartner_ReturnsUnknownTeam()
    {
        var startingPlayer = Substitute.For<IStartingPlayer>();
        var opposingPartner = Substitute.For<IOpposingPartner>();
        var builder = BuildTeamBuilder();

        var result = builder.WithPlayer(startingPlayer)
                            .WithPartner(opposingPartner)
                            .Build();

        Assert.IsType<UnknownTeam>(result);
    }


    [Fact]
    public void OpposingPlayer_StartingPartner_ReturnsUnknownTeam()
    {
        var opposingPlayer = Substitute.For<IOpposingPlayer>();
        var startingPartner = Substitute.For<IStartingPartner>();
        var builder = BuildTeamBuilder();

        var result = builder.WithPlayer(opposingPlayer)
                            .WithPartner(startingPartner)
                            .Build();

        Assert.IsType<UnknownTeam>(result);
    }

    private static ITeamBuilder BuildTeamBuilder() => new TeamBuilder();
}
