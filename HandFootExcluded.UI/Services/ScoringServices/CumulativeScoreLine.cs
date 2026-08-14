using System.Text.Json;
using System.Text.Json.Serialization;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface ICumulativeScoreLine : IRoundScoreLine
{
    IEnumerable<IRoundTotalScoreLine> RoundTotalScoreLines { get; }
}

internal sealed class CumulativeScoreLine : RoundScoreLineBase, ICumulativeScoreLine
{
    [JsonIgnore]
    public override int Order => 5;
    [JsonIgnore]
    public override string Name => "Cumulative";
    [JsonIgnore]
    protected override string Display => $"{Initials} {Name}: {Value}";
    [JsonIgnore]
    public override bool IsBold => true;
    [JsonIgnore]
    public override int FontSize => 18;

    public IEnumerable<IRoundTotalScoreLine> RoundTotalScoreLines { get; set; }

    public CumulativeScoreLine() : base() { }
    public CumulativeScoreLine(int roundOrder, string initials, IEnumerable<IRoundTotalScoreLine> roundTotalScoreLines) : base(roundOrder, initials, GetValue(roundTotalScoreLines)) => RoundTotalScoreLines = roundTotalScoreLines;

    private static int GetValue(IEnumerable<IRoundTotalScoreLine> roundTotalScoreLines) => roundTotalScoreLines.Sum(l => l.Value);
}

public class CumulativeScoreLineConverter : JsonConverter<ICumulativeScoreLine>
{
    public override ICumulativeScoreLine Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<CumulativeScoreLine>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, ICumulativeScoreLine value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}