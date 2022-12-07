using System.Collections;

namespace HandFootExcluded.Core.RoundServices;

public sealed class UnknownRounds : IRounds
{
    public static readonly UnknownRounds Instance = new();
    private static readonly IEnumerable<IRound> _rounds = Enumerable.Empty<IRound>();

    public IEnumerator<IRound> GetEnumerator() => _rounds.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IRounds Add(IRound round) => this;
    public void AddRange(IEnumerable<IRound> rounds) { }

    private UnknownRounds() { }
}