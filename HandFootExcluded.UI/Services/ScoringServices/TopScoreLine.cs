using System.Text.Json;
using System.Text.Json.Serialization;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface ITopScoreLine : IRoundScoreLine { }

internal sealed class TopScoreLine : RoundScoreLineBase, ITopScoreLine
{
    [JsonIgnore]
    public override int Order => 2;
    [JsonIgnore]
    public override string Name => "Top";

    public TopScoreLine() : base() { }
    public TopScoreLine(int roundOrder, string initials, int value) : base(roundOrder, initials, value) { }
}

public class TopScoreLineConverter : JsonConverter<ITopScoreLine>
{
    public override ITopScoreLine Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<TopScoreLine>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, ITopScoreLine value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}