using System.Diagnostics;
using System.Text;

namespace HandFootExcluded.Core.PlayerServices;

public interface INonPositionalPlayer
{
    string FullName { get; }
    string FirstName { get; }
    string MiddleName { get; }
    string LastName { get; }
    string Initials { get; }
}

[DebuggerDisplay("{Display,nq}")]
internal abstract record NonPositionalPlayerBase(string FullName, string FirstName, string MiddleName, string LastName, string Initials) : INonPositionalPlayer
{
    private string Display => Initials.PadRight(3, ' ');

    protected static string GetInitials(string firstName, string middleName, string lastName)
    {
        var initials = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(firstName)) initials.Append(firstName[0]);
        if (!string.IsNullOrWhiteSpace(middleName)) initials.Append(middleName[0]);
        if (!string.IsNullOrWhiteSpace(lastName)) initials.Append(lastName[0]);
        return initials.ToString();
    }

    protected static string GetFullName(string firstName, string middleName, string lastName)
    {
        var fullName = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(firstName)) fullName.Append($"{firstName}");
        if (!string.IsNullOrWhiteSpace(middleName)) fullName.Append($" {middleName}");
        if (!string.IsNullOrWhiteSpace(lastName)) fullName.Append($" {lastName}");
        return fullName.ToString();
    }

    public override string ToString() => Display;
}
