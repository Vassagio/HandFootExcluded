using HandFootExcluded.Services;

namespace HandFootExcluded;

public sealed class UnknownPlayer : IPlayer
{
    public static readonly UnknownPlayer Instance = new();

    public int Position => 0;
    public string FullName => string.Empty;
    public string FirstName => string.Empty;
    public string MiddleName => string.Empty;
    public string LastName => string.Empty;
    public string Initials => string.Empty;

    private UnknownPlayer() { }
}