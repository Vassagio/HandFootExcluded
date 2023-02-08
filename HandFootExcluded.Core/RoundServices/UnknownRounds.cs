using System.Collections;

namespace HandFootExcluded.Core.RoundServices;

public sealed class UnknownRounds : IRounds
{
    public static readonly UnknownRounds Instance = new();
    private static readonly IEnumerable<IRound> Rounds = Enumerable.Empty<IRound>();

    private UnknownRounds() { }

    public IEnumerator<IRound> GetEnumerator() => Rounds.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IRounds Add(IRound round) => this;
    public void AddRange(IEnumerable<IRound> rounds) { }
}