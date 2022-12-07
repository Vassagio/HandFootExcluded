using HandFootExcluded.Core.PlayerServices;
using NSubstitute;

namespace HandFootExcluded.Core.Tests.PlayerServices;

public class PositionalPlayerFactoryTest
{
    private interface INobody : IPositionalPlayer { }

    [Fact]
    public void Initialize()
    {
        var factory = BuildPositionalPlayerFactory();

        Assert.IsAssignableFrom<IPositionalPlayerFactory>(factory);
    }

    [Fact]
    public void Create_StartingPlayer_ReturnStartingPlayer() => AssertPlayer<IStartingPlayer>(PlayerPositionType.StartingPlayer);

    [Fact]
    public void Create_StartingPartner_ReturnStartingPartner() => AssertPlayer<IStartingPartner>(PlayerPositionType.StartingPartner);

    [Fact]
    public void Create_OpposingPlayer_ReturnOpposingPlayer() => AssertPlayer<IOpposingPlayer>(PlayerPositionType.OpposingPlayer);

    [Fact]
    public void Create_OpposingPartner_ReturnOpposingPartner() => AssertPlayer<IOpposingPartner>(PlayerPositionType.OpposingPartner);

    [Fact]
    public void Create_ExcludedPlayer_ReturnExcludedPlayer() => AssertPlayer<IExcludedPlayer>(PlayerPositionType.ExcludedPlayer);

    [Fact]
    public void Create_Nobody_ReturnUnknownPlayer()
    {
        var nonPositionalPlayer = Substitute.For<IOrderedPlayer>();
        var factory = BuildPositionalPlayerFactory();

        var result = factory.Create<INobody>(nonPositionalPlayer);

        Assert.IsType<UnknownPlayer>(result);
        Assert.Equal(PlayerPositionType.None, result.Position);
    }

    private static void AssertPlayer<T>(PlayerPositionType expectedPosition) where T : IPositionalPlayer
    {
        var nonPositionalPlayer = Substitute.For<IOrderedPlayer>();
        var factory = BuildPositionalPlayerFactory();

        var result = factory.Create<T>(nonPositionalPlayer);

        Assert.IsAssignableFrom<T>(result);
        Assert.Equal(expectedPosition, result.Position);
    }

    private static PositionalPlayerFactory BuildPositionalPlayerFactory() => new();
}