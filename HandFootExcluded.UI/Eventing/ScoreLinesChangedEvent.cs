using HandFootExcluded.UI.Services.ScoringServices;

namespace HandFootExcluded.UI.Eventing;

internal sealed class ScoreLinesChangedEvent
{
    public IScoreLines ScoreLines { get; }

    public ScoreLinesChangedEvent(IScoreLines scoreLines) => ScoreLines = scoreLines;
}