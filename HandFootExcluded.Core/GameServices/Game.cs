using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HandFootExcluded.Core.GameServices;

public interface IGame
{
    IRounds Rounds { get; }
    IOrderedEnumerable<IOrderedPlayer> OrderedPlayers { get; }
}

internal sealed class Game : IGame
{
    public Game(IRounds rounds, IOrderedEnumerable<IOrderedPlayer> orderedPlayers)
    {
        Rounds = rounds;
        OrderedPlayers = orderedPlayers;
    }

    public IRounds Rounds { get; }
    public IOrderedEnumerable<IOrderedPlayer> OrderedPlayers { get; }
}

public class GameConverter : JsonConverter<IGame>
{
    public override IGame Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<Game>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, IGame value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (Game)value, options);
    }
}