namespace HandFootExcluded.Core.PlayerServices;

public interface IPlayerSetting
{
    string Name { get; }
    bool Excluded { get; }
}

public sealed record PlayerSetting : IPlayerSetting
{
    public static IPlayerSetting Create(string name, bool excluded = false) => new PlayerSetting(name, excluded);

    public string Name { get; }
    public bool Excluded { get; }

    internal PlayerSetting(string name, bool excluded = false)
    {
        Name = name;
        Excluded = excluded;
    }
}