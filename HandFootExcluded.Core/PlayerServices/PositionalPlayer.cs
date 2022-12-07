namespace HandFootExcluded.Core.PlayerServices;

public interface IPositionalPlayer : INonPositionalPlayer
{
    int Position { get; }
}

internal abstract record PositionalPlayerBase(int Position, string FullName, string FirstName, string MiddleName, string LastName, string Initials) : NonPositionalPlayerBase(FullName, FirstName, MiddleName, LastName, Initials), IPositionalPlayer { }