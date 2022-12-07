using static System.Formats.Asn1.AsnWriter;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IRoundScoreLine : IScoreLine
{
    int RoundOrder { get; }
}


internal abstract class RoundScoreLineBase : ScoreLineBase, IRoundScoreLine
{
    public int RoundOrder { get; }
    protected override string Display => $"{Initials} {Name} {RoundOrder}: {Value}";

    protected RoundScoreLineBase(int roundOrder, string initials, int value) : base(initials, value) => RoundOrder = roundOrder;
}