namespace HandFootExcluded.Core.PlayerServices;

internal sealed partial class NonPositionalPlayerBuilder 
{
    private record Player(string FirstName, string MiddleName, string LastName, bool IsExcludedFirst = false) : NonPositionalPlayerBase(GetFullName(FirstName, MiddleName, LastName), FirstName, MiddleName, LastName, GetInitials(FirstName, MiddleName, LastName), IsExcludedFirst) { }
}