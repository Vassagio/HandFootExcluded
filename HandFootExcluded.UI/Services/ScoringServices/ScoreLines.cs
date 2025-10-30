using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IScoreLines : IEnumerable<IScoreLine>
{
    IScoreLines Add(IScoreLine line);
    IScoreLines Add(IScoreLines lines);
    IEnumerable<string> GetPlayers();
    IEnumerable<int> GetRoundOrders();
    IScoreLines GetTotalScoreLines();
    IScoreLines GetTotalScoreLines(int roundOrder);
    IScoreLines GetCumulativeLines();
    IScoreLines GetGrandTotalLines();
}

internal sealed class ScoreLines : IScoreLines
{
    private readonly ISet<IScoreLine> _lines = new HashSet<IScoreLine>();

    public ScoreLines() { }

    internal ScoreLines(IEnumerable<IScoreLine> scoreLines)
    {
        foreach (var scoreLine in scoreLines)
            Add(scoreLine);
    }

    public IScoreLines Add(IScoreLine line)
    {
        _lines.Add(line);

        return this;
    }

    public IScoreLines Add(IScoreLines lines)
    {
        foreach (var line in lines)
            Add(line);

        return this;
    }

    public IEnumerable<string> GetPlayers()
    {
        return _lines.Select(l => l.Initials).Distinct();
    }

    public IEnumerable<int> GetRoundOrders()
    {
        return _lines.OfType<IRoundScoreLine>().Select(l => l.RoundOrder).Distinct();
    }

    public IScoreLines GetTotalScoreLines(int roundOrder)
    {
        var roundTotalScoreLines = _lines.OfType<IRoundTotalScoreLine>();

        return new ScoreLines(roundTotalScoreLines.Where(l => l.RoundOrder == roundOrder).OrderByDescending(l => l.Value));
    }

    public IScoreLines GetTotalScoreLines()
    {
        var roundTotalScoreLines = _lines.OfType<IRoundTotalScoreLine>();

        return new ScoreLines(roundTotalScoreLines.OrderBy(l => l.RoundOrder).ThenByDescending(l => l.Value));
    }

    public IScoreLines GetCumulativeLines()
    {
        var cumulativeScoreLines = _lines.OfType<ICumulativeScoreLine>();

        return new ScoreLines(cumulativeScoreLines.OrderByDescending(l => l.Value));
    }

    public IScoreLines GetGrandTotalLines()
    {
        var grandTotalScoreLines = _lines.OfType<IGrandTotalScoreLine>();

        return new ScoreLines(grandTotalScoreLines.OrderByDescending(l => l.Value));
    }

    public IEnumerator<IScoreLine> GetEnumerator()
    {
        return _lines.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class ScoreLinesConverter : JsonConverter<IScoreLines>
{
    public override IScoreLines Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var items = new ScoreLines();

        try
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                using var doc = JsonDocument.ParseValue(ref reader);
                var type = doc.RootElement.GetProperty("Type").GetString();

                IScoreLine item = type switch
                                  {
                                      nameof(BonusScoreLine) => JsonSerializer.Deserialize<IBonusScoreLine>(doc.RootElement.GetRawText(), options),
                                      nameof(BottomScoreLine) => JsonSerializer.Deserialize<IBottomScoreLine>(doc.RootElement.GetRawText(), options),
                                      nameof(CumulativeScoreLine) => JsonSerializer.Deserialize<ICumulativeScoreLine>(doc.RootElement.GetRawText(), options),
                                      nameof(GrandTotalScoreLine) => JsonSerializer.Deserialize<IGrandTotalScoreLine>(doc.RootElement.GetRawText(), options),
                                      nameof(RoundTotalScoreLine) => JsonSerializer.Deserialize<IRoundTotalScoreLine>(doc.RootElement.GetRawText(), options),
                                      nameof(TopScoreLine) => JsonSerializer.Deserialize<ITopScoreLine>(doc.RootElement.GetRawText(), options),
                                      _ => throw new JsonException($"Unknown score line type: {type}")
                                  };

                items.Add(item);
            }
        }
        catch (Exception ex)
        {
            return items;
        }

        return items;
    }


    public override void Write(Utf8JsonWriter writer, IScoreLines value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var item in value)
        {
            writer.WriteStartObject();

            writer.WriteString("Type", item.GetType().Name);

            using var doc = JsonDocument.Parse(JsonSerializer.Serialize(item, item.GetType(), options));
            foreach (var property in doc.RootElement.EnumerateObject())
                property.WriteTo(writer);

            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }

}