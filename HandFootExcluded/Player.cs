using System.Diagnostics;
using System.Text;

namespace HandFootExcluded;

public interface IPlayer
{
    int Position { get; }
    string FullName { get; }
    string FirstName { get; }
    string MiddleName { get; }
    string LastName { get; }
    string Initials { get; }
}

internal sealed partial class PlayerBuilder
{
    private abstract record PlayerBase(int Position, string FullName, string FirstName, string MiddleName, string LastName, string Initials) : IPlayer;

    [DebuggerDisplay("{Display,nq}")]
    private sealed record Player(int Position, string FirstName, string MiddleName, string LastName) : PlayerBase(Position, GetFullName(FirstName, MiddleName, LastName), FirstName, MiddleName, LastName, GetInitials(FirstName, MiddleName, LastName))
    {
        private string Display => Initials.PadRight(3, ' ');

        private static string GetInitials(string firstName, string middleName, string lastName)
        {
            var initials = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(firstName)) initials.Append(firstName[0]);
            if (!string.IsNullOrWhiteSpace(middleName)) initials.Append(middleName[0]);
            if (!string.IsNullOrWhiteSpace(lastName)) initials.Append(lastName[0]);
            return initials.ToString();
        }

        private static string GetFullName(string firstName, string middleName, string lastName)
        {
            var fullName = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(firstName)) fullName.Append($"{firstName}");
            if (!string.IsNullOrWhiteSpace(middleName)) fullName.Append($" {middleName}");
            if (!string.IsNullOrWhiteSpace(lastName)) fullName.Append($" {lastName}");
            return fullName.ToString();
        }

        public override string ToString() => Display;
    }
}