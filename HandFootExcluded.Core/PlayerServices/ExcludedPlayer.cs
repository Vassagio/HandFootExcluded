namespace HandFootExcluded.Core.PlayerServices;

public interface IExcludedPlayer : IPositionalPlayer
{
}

internal sealed partial class PositionalPlayerFactory
{
    private record ExcludedPlayer(IOrderedPlayer OrderedPlayer) : PositionalPlayerBase(PlayerPositionType.ExcludedPlayer, OrderedPlayer.Order, OrderedPlayer.FullName, OrderedPlayer.FirstName, OrderedPlayer.MiddleName, OrderedPlayer.LastName, OrderedPlayer.Initials), IExcludedPlayer
    {
        protected override string Display => $"Excluded Player: {Initials,-3}";
    }
}