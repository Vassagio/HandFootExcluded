namespace HandFootExcluded.ScoreLines;

public sealed record GameTotalScore(IPlayer Player, int Value) : ScoreLineBase(Player, Value, int.MaxValue, "Game")
{
}