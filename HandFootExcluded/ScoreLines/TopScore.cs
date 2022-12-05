using HandFootExcluded.Services;

namespace HandFootExcluded.ScoreLines;

public sealed record TopScore(IPlayer Player, IRound Round, int Value) : RoundScoreLineBase(Player, Round, Value, 2, "Top")
{
}