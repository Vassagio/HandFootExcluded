namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IBottomScoreLine : IRoundScoreLine { }

internal sealed class BottomScoreLine : RoundScoreLineBase, IBottomScoreLine
{
    public override int Order => 3;
    public override string Name => "Bottom";
    public BottomScoreLine(int roundOrder, string initials, int value) : base(roundOrder, initials, value) { }
}