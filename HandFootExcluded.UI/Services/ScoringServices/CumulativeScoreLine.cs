﻿namespace HandFootExcluded.UI.Services.ScoringServices;

public interface ICumulativeScoreLine : IRoundScoreLine
{
    IEnumerable<IRoundTotalScoreLine> RoundTotalScoreLines { get; }
}

internal sealed class CumulativeScoreLine : RoundScoreLineBase, ICumulativeScoreLine
{
    public override int Order => 5;
    public override string Name => "Cumulative";
    protected override string Display => $"{Initials} {Name}: {Value}";
    public IEnumerable<IRoundTotalScoreLine> RoundTotalScoreLines { get; }
    public override bool IsBold => true;
    public override int FontSize => 18;

    public CumulativeScoreLine(int roundOrder, string initials, IEnumerable<IRoundTotalScoreLine> roundTotalScoreLines) : base(roundOrder, initials, GetValue(roundTotalScoreLines)) => RoundTotalScoreLines = roundTotalScoreLines;

    private static int GetValue(IEnumerable<IRoundTotalScoreLine> roundTotalScoreLines) => roundTotalScoreLines.Sum(l => l.Value);
}