using System.Diagnostics;

namespace HandFootExcluded.Core.PlayerServices;

public enum PlayerPositionType
{
    None = 0,
    StartingPlayer = 1,
    StartingPartner = 2,
    OpposingPlayer = 3,
    OpposingPartner = 4,
    ExcludedPlayer = 5
}

public interface IPositionalPlayer : IOrderedPlayer
{
    PlayerPositionType Position { get; }
}

[DebuggerDisplay("{Display,nq}")]
internal abstract record PositionalPlayerBase(PlayerPositionType Position, int Order, string FullName, string FirstName, string MiddleName, string LastName, string Initials) : OrderedPlayer(Order, FullName, FirstName, MiddleName, LastName, Initials), IPositionalPlayer
{
    protected abstract string Display { get; }
    public override string ToString() => Display;
}