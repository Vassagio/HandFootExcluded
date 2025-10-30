using System.Text.Json;
using System.Text.Json.Serialization;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IBottomScoreLine : IRoundScoreLine { }

internal sealed class BottomScoreLine : RoundScoreLineBase, IBottomScoreLine
{
    [JsonIgnore]
    public override int Order => 3;
    [JsonIgnore]
    public override string Name => "Bottom";

    public BottomScoreLine() : base() { }
    public BottomScoreLine(int roundOrder, string initials, int value) : base(roundOrder, initials, value) { }
}

public sealed class BottomScoreLineConverter : JsonConverter<IBottomScoreLine>
{
    public override IBottomScoreLine Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<BottomScoreLine>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, IBottomScoreLine value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}