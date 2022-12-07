namespace HandFootExcluded.Core.PlayerServices;

public interface IStartingPlayer : IPositionalPlayer { }

internal sealed partial class PositionalPlayerFactory
{
    private record StartingPlayer(INonPositionalPlayer NonPositionalPlayer) : PositionalPlayerBase(1, NonPositionalPlayer.FullName, NonPositionalPlayer.FirstName, NonPositionalPlayer.MiddleName, NonPositionalPlayer.LastName, NonPositionalPlayer.Initials), IStartingPlayer { }
}