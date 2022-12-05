using System.Diagnostics;
using HandFootExcluded.Services;

namespace HandFootExcluded.ScoreLines;

public interface IScoreLine
{
    int DisplayOrder { get; }
    string DisplayName { get; }
    IPlayer Player { get; }
    int Value { get; }
}

[DebuggerDisplay("{Display,nq}")]
public abstract record ScoreLineBase(IPlayer Player, int Value, int DisplayOrder, string DisplayName) : IScoreLine
{
    private string Display => $"Game Total: {Player.FirstName} - {Value}";

    public override string ToString() => Display;
}