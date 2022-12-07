namespace HandFootExcluded.UI.Services.ScoringServices;

public interface ITopScoreLine : IRoundScoreLine { }

internal sealed class TopScoreLine : RoundScoreLineBase, ITopScoreLine
{
    public override string Name => "Top";
    public TopScoreLine(int roundOrder, string initials, int value) : base(roundOrder, initials, value) { }
}