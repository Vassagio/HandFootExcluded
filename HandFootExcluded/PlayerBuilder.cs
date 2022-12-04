namespace HandFootExcluded;

public interface IPlayerBuilder : IBuilder
{
    IPlayerBuilderPosition WithPosition(int position);
}

public interface IPlayerBuilderPosition : IBuilder
{
    IPlayerBuilderBuild WithName(string name);
}

public interface IPlayerBuilderBuild : IBuilder
{
    IPlayer Build();
}

internal sealed partial class PlayerBuilder : BuilderBase<PlayerBuilder>, IPlayerBuilder, IPlayerBuilderPosition, IPlayerBuilderBuild
{
    private string _name = string.Empty;
    private int _position = 1;
    public IPlayerBuilderPosition WithPosition(int position) => SetProperty(ref _position, position);

    public IPlayer Build()
    {
        if (_position <= 0) return UnknownPlayer.Instance;
        if (string.IsNullOrWhiteSpace(_name)) return UnknownPlayer.Instance;

        var parsedName = Parse(_name);

        return new Player(_position, parsedName.FirstName, parsedName.MiddleName, parsedName.LastName);
    }

    public IPlayerBuilderBuild WithName(string name) => SetProperty(ref _name, name);

    private (string FirstName, string MiddleName, string LastName) Parse(string name)
    {
        var parsedName = _name.Split(' ').ToList();
        return parsedName.Count switch
        {
            1 => (parsedName.First(), string.Empty, string.Empty),
            2 => (parsedName.First(), string.Empty, parsedName.Last()),
            3 => (parsedName.First(), parsedName[1].Replace(".", string.Empty), parsedName.Last()),
            _ => (parsedName.First(), string.Empty, parsedName.Last())
        };
    }
}