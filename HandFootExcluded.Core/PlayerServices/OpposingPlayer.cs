namespace HandFootExcluded.Core.PlayerServices;

public interface IOpposingPlayer : IPositionalPlayer { }

internal sealed partial class PositionalPlayerFactory
{
    private record OpposingPlayer(INonPositionalPlayer NonPositionalPlayer) : PositionalPlayerBase(3, NonPositionalPlayer.FullName, NonPositionalPlayer.FirstName, NonPositionalPlayer.MiddleName, NonPositionalPlayer.LastName, NonPositionalPlayer.Initials), IOpposingPlayer { }
}