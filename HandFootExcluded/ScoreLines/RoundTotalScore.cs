namespace HandFootExcluded.ScoreLines;

public sealed record RoundTotalScore(IPlayer Player, IRound Round, int Value) : RoundScoreLineBase(Player, Round, Value, 4, "Round")
{
}