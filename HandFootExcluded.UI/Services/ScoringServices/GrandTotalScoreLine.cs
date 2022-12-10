namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IGrandTotalScoreLine : IScoreLine
{
    ICumulativeScoreLine CumulativeScoreLine { get; }
}

internal sealed class GrandTotalScoreLine : ScoreLineBase, IGrandTotalScoreLine
{
    public override int Order => int.MaxValue;
    public override string Name => "Grand Total";
    protected override string Display => $"{Initials} {Name}: {Value}";
    public ICumulativeScoreLine CumulativeScoreLine { get; }
    public override bool IsBold => true;
    public override int FontSize => 20;

    public GrandTotalScoreLine(string initials, ICumulativeScoreLine cumulativeScoreLine) : base(initials, GetValue(cumulativeScoreLine)) => CumulativeScoreLine = cumulativeScoreLine;

    private static int GetValue(ICumulativeScoreLine cumulativeScoreLine) => cumulativeScoreLine.Value;
}