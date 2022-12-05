using HandFootExcluded.Services;

namespace HandFootExcluded.ScoreLines;

public sealed record CumulativeTotalScore(IPlayer Player, IRound Round, int Value) : RoundScoreLineBase(Player, Round, Value, 9999, "Cumulative") { }