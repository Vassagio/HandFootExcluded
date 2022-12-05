using HandFootExcluded.Services;

namespace HandFootExcluded.ScoreLines;

public sealed record BottomScore(IPlayer Player, IRound Round, int Value) : RoundScoreLineBase(Player, Round, Value, 3, "Bottom")
{
}