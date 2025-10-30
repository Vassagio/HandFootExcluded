using HandFootExcluded.Common;

namespace HandFootExcluded.Core.PlayerServices;

public interface INonPositionalPlayerBuilder : IBuilder
{
    INonPositionalPlayerBuilderExclude WithFullName(string fullName);
    INonPositionalPlayerBuilderName WithFirstName(string firstName);
}

public interface INonPositionalPlayerBuilderName : IBuilder
{
    INonPositionalPlayerBuilderName WithMiddleName(string middleName);
    INonPositionalPlayerBuilderExclude WithLastName(string lastName);
}

public interface INonPositionalPlayerBuilderExclude : IBuilder
{
    INonPositionalPlayerBuilderBuild IsExcluded(bool isExcluded);
}

public interface INonPositionalPlayerBuilderBuild : IBuilder<INonPositionalPlayer> { }

internal sealed partial class NonPositionalPlayerBuilder : BuilderBase<NonPositionalPlayerBuilder, INonPositionalPlayer>, INonPositionalPlayerBuilder, INonPositionalPlayerBuilderName, INonPositionalPlayerBuilderExclude, INonPositionalPlayerBuilderBuild
{
    private string _firstName;
    private string _middleName = string.Empty;
    private string _lastName;
    private string _fullName;
    private bool _isExcluded;
    private int _order;

    public INonPositionalPlayerBuilderName WithFirstName(string firstName) => SetProperty(ref _firstName, firstName);

    public INonPositionalPlayerBuilderExclude WithFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) fullName = string.Empty;

        var name = Parse(fullName);

        return SetProperty(ref _firstName, name.FirstName)
              .SetProperty(ref _middleName, name.MiddleName)
              .SetProperty(ref _lastName, name.LastName)
              .SetProperty(ref _fullName, fullName);
    }

    public INonPositionalPlayerBuilderName WithMiddleName(string middleName) => SetProperty(ref _middleName, middleName);
    public INonPositionalPlayerBuilderExclude WithLastName(string lastName) => SetProperty(ref _lastName, lastName);

    public INonPositionalPlayerBuilderBuild IsExcluded(bool isExcluded) => SetProperty(ref _isExcluded, isExcluded);

    private static (string FirstName, string MiddleName, string LastName) Parse(string name)
    {
        var parsedName = name.Trim().RemoveMultipleSpaces().Split(' ').ToList();
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

        return new Player(_firstName.Trim(), _middleName.Trim(), _lastName.Trim(), _isExcluded);
    }
}