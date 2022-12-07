namespace HandFootExcluded.Core.PlayerServices;

public interface IPositionalPlayerFactory
{
    IPositionalPlayer Create<TPositionalPlayer>(IOrderedPlayer orderedPlayer) where TPositionalPlayer : IPositionalPlayer;
}

internal sealed partial class PositionalPlayerFactory : IPositionalPlayerFactory
{
    public IPositionalPlayer Create<TPositionalPlayer>(IOrderedPlayer orderedPlayer) where TPositionalPlayer : IPositionalPlayer =>
        typeof(TPositionalPlayer).Name switch
        {
            nameof(IStartingPlayer)  => new StartingPlayer(orderedPlayer),
            nameof(IStartingPartner) => new StartingPartner(orderedPlayer),
            nameof(IOpposingPlayer)  => new OpposingPlayer(orderedPlayer),
            nameof(IOpposingPartner) => new OpposingPartner(orderedPlayer),
            nameof(IExcludedPlayer)  => new ExcludedPlayer(orderedPlayer),
            _                        => UnknownPlayer.Instance
        };
}