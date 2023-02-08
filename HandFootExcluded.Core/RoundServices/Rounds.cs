using System.Collections;

namespace HandFootExcluded.Core.RoundServices;

public interface IRounds : IEnumerable<IRound>
{
    IRounds Add(IRound round);
    void AddRange(IEnumerable<IRound> rounds);
}

internal sealed class Rounds : IRounds
{
    private readonly ISet<IRound> _rounds = new HashSet<IRound>();

    public Rounds() { }

    public Rounds(IRound round, params IRound[] rounds) : this()
    {
        Add(round);
        AddRange(rounds);
    }

    public IRounds Add(IRound round)
    {
        if (round is null or UnknownRound) return this;

        _rounds.Add(round);

        return this;
    }

    public void AddRange(IEnumerable<IRound> rounds)
    {
        foreach (var round in rounds.Where(p => p is not UnknownRound))
            _rounds.Add(round);
    }

    public IEnumerator<IRound> GetEnumerator() => _rounds.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}