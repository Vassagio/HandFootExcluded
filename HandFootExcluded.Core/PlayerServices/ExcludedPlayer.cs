namespace HandFootExcluded.Core.PlayerServices;

public interface IExcludedPlayer : IPositionalPlayer { }

internal sealed partial class PositionalPlayerFactory
{
    private record ExcludedPlayer(INonPositionalPlayer NonPositionalPlayer) : PositionalPlayerBase(5, NonPositionalPlayer.FullName, NonPositionalPlayer.FirstName, NonPositionalPlayer.MiddleName, NonPositionalPlayer.LastName, NonPositionalPlayer.Initials), IExcludedPlayer { }
}