﻿using HandFootExcluded.UI.Services.ScoringServices;

namespace HandFootExcluded.UI.Eventing;

internal sealed class RoundScoreLinesChangedEvent
{
    public int RoundOrder { get; }
    public IScoreLines ScoreLines { get; }

    public RoundScoreLinesChangedEvent(int roundOrder, IScoreLines scoreLines)
    {
        RoundOrder = roundOrder;
        ScoreLines = scoreLines.GetTotalScoreLines(roundOrder);
    }
}