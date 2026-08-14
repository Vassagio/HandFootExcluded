using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Formats.Asn1.AsnWriter;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IBonusScoreLine : IRoundScoreLine { }

internal sealed class BonusScoreLine : RoundScoreLineBase, IBonusScoreLine
{
    [JsonIgnore]
    public override int Order => 1;
    [JsonIgnore]
    public override string Name => "Bonus";

    public BonusScoreLine() : base() { }
    public BonusScoreLine(int roundOrder, string initials, bool hasBonus, int bonusAmount) : base(roundOrder, initials, hasBonus ? bonusAmount : 0 ) { }
}

public sealed class BonusScoreLineConverter : JsonConverter<IBonusScoreLine>
{
    public override IBonusScoreLine Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<BonusScoreLine>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, IBonusScoreLine value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}