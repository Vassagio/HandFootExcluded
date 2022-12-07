namespace HandFootExcluded.Core.PlayerServices;

public interface IOpposingPartner : IPositionalPlayer { }

internal sealed partial class PositionalPlayerFactory
{
    private record OpposingPartner(INonPositionalPlayer NonPositionalPlayer) : PositionalPlayerBase(4, NonPositionalPlayer.FullName, NonPositionalPlayer.FirstName, NonPositionalPlayer.MiddleName, NonPositionalPlayer.LastName, NonPositionalPlayer.Initials), IOpposingPartner { }
}