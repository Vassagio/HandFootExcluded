using HandFootExcluded.UI.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IGrandTotalScoreLine : IScoreLine
{
    ICumulativeScoreLine CumulativeScoreLine { get; }
}

internal sealed class GrandTotalScoreLine : ScoreLineBase, IGrandTotalScoreLine
{
    [JsonIgnore]
    public override int Order => int.MaxValue;
    [JsonIgnore]
    public override string Name => "Grand Total";
    [JsonIgnore]
    protected override string Display => $"{Initials} {Name}: {Value}";
    [JsonIgnore]
    public override bool IsBold => true;
    [JsonIgnore]
    public override int FontSize => 20;

    public ICumulativeScoreLine CumulativeScoreLine { get; set; }

    public GrandTotalScoreLine() : base() {}
    public GrandTotalScoreLine(string initials, ICumulativeScoreLine cumulativeScoreLine) : base(initials, GetValue(cumulativeScoreLine)) => CumulativeScoreLine = cumulativeScoreLine;

    private static int GetValue(ICumulativeScoreLine cumulativeScoreLine) => cumulativeScoreLine.Value;
}

public sealed class GrandTotalScoreLineConverter : JsonConverter<IGrandTotalScoreLine>
{
    public override IGrandTotalScoreLine Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result =  JsonSerializer.Deserialize<GrandTotalScoreLine>(ref reader, options);
        return result;
    }

    public override void Write(Utf8JsonWriter writer, IGrandTotalScoreLine value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}