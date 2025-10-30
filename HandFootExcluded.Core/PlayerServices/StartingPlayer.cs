namespace HandFootExcluded.Core.PlayerServices;

public interface IStartingPlayer : IPositionalPlayer { }

internal record StartingPlayer(IOrderedPlayer OrderedPlayer) : PositionalPlayerBase(PlayerPositionType.StartingPlayer, OrderedPlayer.Order, OrderedPlayer.FullName, OrderedPlayer.FirstName, OrderedPlayer.MiddleName, OrderedPlayer.LastName, OrderedPlayer.Initials), IStartingPlayer
{
    protected override string Display => $"Starting Player: {Initials,-3}";
}