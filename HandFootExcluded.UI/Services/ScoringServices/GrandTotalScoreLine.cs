namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IGrandTotalScoreLine : IScoreLine
{
    IEnumerable<ICumulativeScoreLine> CumulativeScoreLines { get; }
}

internal sealed class GrandTotalScoreLine : ScoreLineBase, IGrandTotalScoreLine
{
    public override string Name => "Grand Total";
    protected override string Display => $"{Initials} {Name}: {Value}";
    public IEnumerable<ICumulativeScoreLine> CumulativeScoreLines { get; }

    public GrandTotalScoreLine(string initials, IEnumerable<ICumulativeScoreLine> cumulativeScoreLines) : base(initials, GetValue(cumulativeScoreLines)) => CumulativeScoreLines = cumulativeScoreLines;

    private static int GetValue(IEnumerable<ICumulativeScoreLine> cumulativeScoreLines) => cumulativeScoreLines.Sum(l => l.Value);
}