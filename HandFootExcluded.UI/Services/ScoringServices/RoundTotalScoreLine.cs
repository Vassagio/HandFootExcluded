namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IRoundTotalScoreLine : IRoundScoreLine
{
    IBonusScoreLine BonusScore { get; }
    ITopScoreLine TopScore { get; }
    IBottomScoreLine BottomScore { get; }
}

internal sealed class RoundTotalScoreLine : RoundScoreLineBase, IRoundTotalScoreLine
{
    public override int Order => 4;
    public override string Name => "Round";
    public IBonusScoreLine BonusScore { get; }
    public ITopScoreLine TopScore { get; }
    public IBottomScoreLine BottomScore { get; }

    public RoundTotalScoreLine(int roundOrder, string initials, IBonusScoreLine bonusScoreLine, ITopScoreLine topScoreLine, IBottomScoreLine bottomScoreLine) : base(roundOrder, initials, GetValue(bonusScoreLine, topScoreLine, bottomScoreLine))
    {
        BonusScore = bonusScoreLine;
        TopScore = topScoreLine;
        BottomScore = bottomScoreLine;
    }

    private static int GetValue(IBonusScoreLine bonusScoreLine, ITopScoreLine topScoreLine, IBottomScoreLine bottomScoreLine) => bonusScoreLine.Value + topScoreLine.Value + bottomScoreLine.Value;
}