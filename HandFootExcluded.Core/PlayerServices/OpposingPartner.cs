namespace HandFootExcluded.Core.PlayerServices;

public interface IOpposingPartner : IPositionalPlayer { }

internal sealed partial class PositionalPlayerFactory
{
    private record OpposingPartner(IOrderedPlayer OrderedPlayer) : PositionalPlayerBase(PlayerPositionType.OpposingPartner, OrderedPlayer.Order, OrderedPlayer.FullName, OrderedPlayer.FirstName, OrderedPlayer.MiddleName, OrderedPlayer.LastName, OrderedPlayer.Initials), IOpposingPartner
    {
        protected override string Display => $"Opposing Partner: {Initials,-3}";
    }
}