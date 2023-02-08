namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IRoundScoreLine : IScoreLine
{
    int RoundOrder { get; }
}

internal abstract class RoundScoreLineBase : ScoreLineBase, IRoundScoreLine
{
    protected override string Display => $"{Initials} {Name} {RoundOrder}: {Value}";

    protected RoundScoreLineBase(int roundOrder, string initials, int value) : base(initials, value) => RoundOrder = roundOrder;
    public int RoundOrder { get; }
}