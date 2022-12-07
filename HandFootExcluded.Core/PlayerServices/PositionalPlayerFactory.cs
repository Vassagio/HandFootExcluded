namespace HandFootExcluded.Core.PlayerServices;

public interface IPositionalPlayerFactory
{
    IPositionalPlayer Create<TPositionalPlayer>(INonPositionalPlayer nonPositionalPlayer) where TPositionalPlayer : IPositionalPlayer
}

internal sealed partial class PositionalPlayerFactory
{
    public IPositionalPlayer Create<TPositionalPlayer>(INonPositionalPlayer nonPositionalPlayer) where TPositionalPlayer : IPositionalPlayer =>
        typeof(TPositionalPlayer).Name switch
        {
            nameof(IStartingPlayer)  => new StartingPlayer(nonPositionalPlayer),
            nameof(IStartingPartner) => new StartingPartner(nonPositionalPlayer),
            nameof(IOpposingPlayer)  => new OpposingPlayer(nonPositionalPlayer),
            nameof(IOpposingPartner) => new OpposingPartner(nonPositionalPlayer),
            nameof(IExcludedPlayer)  => new ExcludedPlayer(nonPositionalPlayer),
            _                        => UnknownPlayer.Instance
        };
}