namespace HandFootExcluded.UI.Services.ScoringServices;

public interface ITopScoreLine : IRoundScoreLine
{
}

internal sealed class TopScoreLine : RoundScoreLineBase, ITopScoreLine
{
    public TopScoreLine(int roundOrder, string initials, int value) : base(roundOrder, initials, value) { }
    public override int Order => 2;
    public override string Name => "Top";
}