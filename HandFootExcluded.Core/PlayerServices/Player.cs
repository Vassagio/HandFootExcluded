namespace HandFootExcluded.Core.PlayerServices;

internal sealed partial class NonPositionalPlayerBuilder
{
    private record Player(string FirstName, string MiddleName, string LastName) : NonPositionalPlayerBase(GetFullName(FirstName, MiddleName, LastName), FirstName, MiddleName, LastName, GetInitials(FirstName, MiddleName, LastName))
    {
    }
}