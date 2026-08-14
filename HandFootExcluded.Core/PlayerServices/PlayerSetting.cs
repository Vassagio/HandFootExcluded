using System.Text.Json;
using System.Text.Json.Serialization;

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

    public PlayerSetting() { }

    internal PlayerSetting(string name, bool excluded = false)
    {
        Name = name;
        Excluded = excluded;
    }
}

public class PlayerSettingConverter : JsonConverter<IPlayerSetting>
{
    public override IPlayerSetting Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<PlayerSetting>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, IPlayerSetting value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (PlayerSetting)value, options);
    }
}
