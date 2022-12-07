namespace HandFootExcluded.Core.PlayerServices;

public interface IStartingPartner : IPositionalPlayer { }

internal sealed partial class PositionalPlayerFactory
{
    private record StartingPartner(INonPositionalPlayer NonPositionalPlayer) : PositionalPlayerBase(2, NonPositionalPlayer.FullName, NonPositionalPlayer.FirstName, NonPositionalPlayer.MiddleName, NonPositionalPlayer.LastName, NonPositionalPlayer.Initials), IStartingPartner { }
}