namespace HandFootExcluded.Core.PlayerServices;

public interface IOpposingPlayer : IPositionalPlayer { }

internal sealed partial class PositionalPlayerFactory
{
    private record OpposingPlayer(IOrderedPlayer OrderedPlayer) : PositionalPlayerBase(PlayerPositionType.OpposingPlayer, OrderedPlayer.Order, OrderedPlayer.FullName, OrderedPlayer.FirstName, OrderedPlayer.MiddleName, OrderedPlayer.LastName, OrderedPlayer.Initials), IOpposingPlayer
    {
        protected override string Display => $"Opposing Player: {Initials,-3}";
    }
}