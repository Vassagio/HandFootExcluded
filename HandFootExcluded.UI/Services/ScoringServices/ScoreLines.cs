using System.Collections;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IScoreLines : IEnumerable<IScoreLine>
{
    IScoreLines Add(IScoreLine line);
    IScoreLines Add(IScoreLines lines);
    IEnumerable<string> GetPlayers();
    IEnumerable<int> GetRoundOrders();
    IScoreLines GetTotalScoreLines();
    IScoreLines GetTotalScoreLines(int roundOrder);
    IScoreLines GetCumulativeLines();
    IScoreLines GetGrandTotalLines();
}

internal sealed class ScoreLines : IScoreLines
{
    private readonly ISet<IScoreLine> _lines = new HashSet<IScoreLine>();

    public ScoreLines() { }
    private ScoreLines(IEnumerable<IScoreLine> scoreLines)
    {
        foreach (var scoreLine in scoreLines)
            Add(scoreLine);
    }

    public IScoreLines Add(IScoreLine line)
    {
        _lines.Add(line);

        return this;
    }

    public IScoreLines Add(IScoreLines lines)
    {
        foreach (var line in lines)
            Add(line);
        return this;
    }

    public IEnumerable<string> GetPlayers() =>
        _lines.Select(l => l.Initials)
              .Distinct();
    public IEnumerable<int> GetRoundOrders() =>
        _lines.OfType<IRoundScoreLine>()
              .Select(l => l.RoundOrder)
              .Distinct();
    public IScoreLines GetTotalScoreLines(int roundOrder)
    {
        var roundTotalScoreLines = _lines.OfType<IRoundTotalScoreLine>();
        return new ScoreLines(roundTotalScoreLines.Where(l => l.RoundOrder == roundOrder)
                                                  .OrderByDescending(l => l.Value));
    }

    public IScoreLines GetTotalScoreLines()
    {
        var roundTotalScoreLines = _lines.OfType<IRoundTotalScoreLine>();
        return new ScoreLines(roundTotalScoreLines.OrderBy(l => l.RoundOrder)
                                                  .ThenByDescending(l => l.Value));
    }

    public IScoreLines GetCumulativeLines()
    {
        var cumulativeScoreLines = _lines.OfType<ICumulativeScoreLine>();
        return new ScoreLines(cumulativeScoreLines.OrderByDescending(l => l.Value));
    }

    public IScoreLines GetGrandTotalLines()
    {
        var grandTotalScoreLines = _lines.OfType<IGrandTotalScoreLine>();
        return new ScoreLines(grandTotalScoreLines.OrderByDescending(l => l.Value));
    }

    public IEnumerator<IScoreLine> GetEnumerator() => _lines.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}