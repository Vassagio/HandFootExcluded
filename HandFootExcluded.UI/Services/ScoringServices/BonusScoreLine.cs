namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IBonusScoreLine : IRoundScoreLine
{
}

internal sealed class BonusScoreLine : RoundScoreLineBase, IBonusScoreLine
{
    public BonusScoreLine(int roundOrder, string initials, bool hasBonus, int bonusAmount) : base(roundOrder, initials, hasBonus ? bonusAmount : 0) { }
    public override int Order => 1;
    public override string Name => "Bonus";
}