namespace HandFootExcluded.Core.PlayerServices;

public interface IPlayerBuilder : IBuilder
{
    IPlayerBuilderBuild WithFullName(string fullName);
    IPlayerBuilderName WithFirstName(string firstName);
}

public interface IPlayerBuilderName : IBuilder
{
    IPlayerBuilderName WithMiddleName(string middleName);
    IPlayerBuilderBuild WithLastName(string lastName);
}

public interface IPlayerBuilderBuild : IBuilder<INonPositionalPlayer>
{

}

internal sealed partial class PlayerBuilder : BuilderBase<PlayerServices.PlayerBuilder, INonPositionalPlayer>, IPlayerBuilder, IPlayerBuilderName, IPlayerBuilderBuild
{
    private string _firstName;
    private string _middleName;
    private string _lastName;
    private string _fullName;

    public IPlayerBuilderName WithFirstName(string firstName) => SetProperty(ref _firstName, firstName);
    public IPlayerBuilderName WithMiddleName(string middleName) => SetProperty(ref _middleName, middleName);
    public IPlayerBuilderBuild WithLastName(string lastName) => SetProperty(ref _lastName, lastName);
    public IPlayerBuilderBuild WithFullName(string fullName)
    {
        var name = Parse(fullName);

        return SetProperty(ref _firstName, name.FirstName)
              .SetProperty(ref _middleName, name.MiddleName)
              .SetProperty(ref _lastName, name.LastName)
              .SetProperty(ref _fullName, fullName);
    }


    private static (string FirstName, string MiddleName, string LastName) Parse(string name)
    {
        var parsedName = name.Split(' ').ToList();
        return parsedName.Count switch
        {
            1 => (parsedName.First(), string.Empty, string.Empty),
            2 => (parsedName.First(), string.Empty, parsedName.Last()),
            3 => (parsedName.First(), parsedName[1].Replace(".", string.Empty), parsedName.Last()),
            _ => (parsedName.First(), string.Empty, parsedName.Last())
        };
    }

    protected override INonPositionalPlayer BuildInternal()
    {
        if (string.IsNullOrWhiteSpace(_firstName)) return UnknownPlayer.Instance;
        if (string.IsNullOrWhiteSpace(_lastName)) return UnknownPlayer.Instance;

        return new PlayerServices.PlayerBuilder.Player(_firstName, _middleName, _lastName);
    }
}
