namespace HandFootExcluded.Core.PlayerServices;

public sealed class UnknownPlayer : IPositionalPlayer
{
    public static readonly UnknownPlayer Instance = new();

    public int Position => -1;
    public string FullName => string.Empty;
    public string FirstName => string.Empty;
    public string MiddleName => string.Empty;
    public string LastName => string.Empty;
    public string Initials => string.Empty;

    private UnknownPlayer() { }
}