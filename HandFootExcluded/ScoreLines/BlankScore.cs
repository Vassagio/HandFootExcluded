using HandFootExcluded.Services;

namespace HandFootExcluded.ScoreLines;

public sealed record BlankScore(IPlayer Player, IRound Round) : RoundScoreLineBase(Player, Round, 0, 10000, string.Empty) { }