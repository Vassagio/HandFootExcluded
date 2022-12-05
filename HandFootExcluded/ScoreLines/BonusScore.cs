namespace HandFootExcluded.ScoreLines;

public sealed record BonusScore(IPlayer Player, IRound Round, int Value) : RoundScoreLineBase(Player, Round, Value, 1, "Bonus")
{
}