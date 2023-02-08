using HandFootExcluded.Common;

namespace HandFootExcluded.Core.PlayerServices;

public interface INonPositionalPlayerBuilder : IBuilder
{
    INonPositionalPlayerBuilderBuild WithFullName(string fullName);
    INonPositionalPlayerBuilderName WithFirstName(string firstName);
}

public interface INonPositionalPlayerBuilderName : IBuilder
{
    INonPositionalPlayerBuilderName WithMiddleName(string middleName);
    INonPositionalPlayerBuilderBuild WithLastName(string lastName);
}

public interface INonPositionalPlayerBuilderBuild : IBuilder<INonPositionalPlayer>
{
}

internal sealed partial class NonPositionalPlayerBuilder : BuilderBase<NonPositionalPlayerBuilder, INonPositionalPlayer>, INonPositionalPlayerBuilder, INonPositionalPlayerBuilderName, INonPositionalPlayerBuilderBuild
{
    private string _firstName;
    private string _fullName;
    private string _lastName;
    private string _middleName = string.Empty;

    public INonPositionalPlayerBuilderName WithFirstName(string firstName) => SetProperty(ref _firstName, firstName);

    public INonPositionalPlayerBuilderBuild WithFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) fullName = string.Empty;

        var name = Parse(fullName);

        return SetProperty(ref _firstName, name.FirstName)
              .SetProperty(ref _middleName, name.MiddleName)
              .SetProperty(ref _lastName, name.LastName)
              .SetProperty(ref _fullName, fullName);
    }

    public INonPositionalPlayerBuilderName WithMiddleName(string middleName) => SetProperty(ref _middleName, middleName);
    public INonPositionalPlayerBuilderBuild WithLastName(string lastName) => SetProperty(ref _lastName, lastName);

    private static (string FirstName, string MiddleName, string LastName) Parse(string name)
    {
        var parsedName = name.Trim()
                             .RemoveMultipleSpaces()
                             .Split(' ')
                             .ToList();
        return parsedName.Count switch
        {
            1 => (parsedName.First(), string.Empty, string.Empty),
            2 => (parsedName.First(), string.Empty, parsedName.Last()),
            3 => (parsedName.First(), parsedName[1], parsedName.Last()),
            _ => (parsedName.First(), string.Empty, parsedName.Last())
        };
    }

    protected override INonPositionalPlayer BuildInternal()
    {
        if (string.IsNullOrWhiteSpace(_firstName)) return UnknownPlayer.Instance;

        if (string.IsNullOrWhiteSpace(_middleName)) _middleName = string.Empty;
        if (string.IsNullOrWhiteSpace(_lastName)) return UnknownPlayer.Instance;

        return new Player(_firstName.Trim(), _middleName.Trim(), _lastName.Trim());
    }
}