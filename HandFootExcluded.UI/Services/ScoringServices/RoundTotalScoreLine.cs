using System.Text.Json;
using System.Text.Json.Serialization;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IRoundTotalScoreLine : IRoundScoreLine
{
    IBonusScoreLine BonusScore { get; }
    ITopScoreLine TopScore { get; }
    IBottomScoreLine BottomScore { get; }
}

internal sealed class RoundTotalScoreLine : RoundScoreLineBase, IRoundTotalScoreLine
{
    [JsonIgnore]
    public override int Order => 4;
    [JsonIgnore]
    public override string Name => "Round";
    [JsonIgnore]
    public override bool IsBold => true;
    [JsonIgnore]
    public override int FontSize => 18;

    public IBonusScoreLine BonusScore { get; set; }
    public ITopScoreLine TopScore { get; set; }
    public IBottomScoreLine BottomScore { get; set; }

    public RoundTotalScoreLine() : base() { }
    public RoundTotalScoreLine(int roundOrder, string initials, IBonusScoreLine bonusScoreLine, ITopScoreLine topScoreLine, IBottomScoreLine bottomScoreLine) : base(roundOrder, initials, GetValue(bonusScoreLine, topScoreLine, bottomScoreLine))
    {
        BonusScore = bonusScoreLine;
        TopScore = topScoreLine;
        BottomScore = bottomScoreLine;
    }

    private static int GetValue(IBonusScoreLine bonusScoreLine, ITopScoreLine topScoreLine, IBottomScoreLine bottomScoreLine) => bonusScoreLine.Value + topScoreLine.Value + bottomScoreLine.Value;
}

public class RoundTotalScoreLineConverter : JsonConverter<IRoundTotalScoreLine>
{
    public override IRoundTotalScoreLine Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<RoundTotalScoreLine>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, IRoundTotalScoreLine value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

public class RoundTotalScoreLinesConverter : JsonConverter<IEnumerable<IRoundTotalScoreLine>>
{
    public override IEnumerable<IRoundTotalScoreLine> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var items = new List<IRoundTotalScoreLine>();

        try
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                using var doc = JsonDocument.ParseValue(ref reader);
                var item = JsonSerializer.Deserialize<IRoundTotalScoreLine>(doc.RootElement.GetRawText(), options);
                items.Add(item);
            }
        }
        catch (Exception ex)
        {
            return items;
        }

        return items;
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<IRoundTotalScoreLine> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var item in value)
        {
            writer.WriteStartObject();

            using var doc = JsonDocument.Parse(JsonSerializer.Serialize(item, item.GetType(), options));
            foreach (var property in doc.RootElement.EnumerateObject())
                property.WriteTo(writer);

            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}