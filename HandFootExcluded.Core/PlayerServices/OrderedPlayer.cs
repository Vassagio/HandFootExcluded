namespace HandFootExcluded.Core.PlayerServices;

public interface IOrderedPlayer : INonPositionalPlayer
{
    int Order { get; }
}

internal record OrderedPlayer(int Order, string FullName, string FirstName, string MiddleName, string LastName, string Initials) : IOrderedPlayer
{
    
}