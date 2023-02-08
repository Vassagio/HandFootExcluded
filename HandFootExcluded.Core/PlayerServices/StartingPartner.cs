namespace HandFootExcluded.Core.PlayerServices;

public interface IStartingPartner : IPositionalPlayer
{
}

internal sealed partial class PositionalPlayerFactory
{
    private record StartingPartner(IOrderedPlayer OrderedPlayer) : PositionalPlayerBase(PlayerPositionType.StartingPartner, OrderedPlayer.Order, OrderedPlayer.FullName, OrderedPlayer.FirstName, OrderedPlayer.MiddleName, OrderedPlayer.LastName, OrderedPlayer.Initials), IStartingPartner
    {
        protected override string Display => $"Staring Partner: {Initials,-3}";
    }
}