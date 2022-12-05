using System.Diagnostics;

namespace HandFootExcluded.ScoreLines;

public interface IRoundScoreLine : IScoreLine
{
    IRound Round { get; }
}

[DebuggerDisplay("{Display,nq}")]
public abstract record RoundScoreLineBase(IPlayer Player, IRound Round, int Value, int DisplayOrder, string DisplayName) : ScoreLineBase(Player, Value, DisplayOrder, DisplayName), IRoundScoreLine
{
    private string Display => $"Round {Round.Index}: {Player.FirstName} - {Value}";

    public override string ToString() => Display;
}