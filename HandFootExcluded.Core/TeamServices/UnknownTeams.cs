using System.Collections;

namespace HandFootExcluded.Core.TeamServices;

public sealed class UnknownTeams : ITeams
{
    public static readonly UnknownTeams Instance = new();
    private static readonly IEnumerable<ITeam> Teams = Enumerable.Empty<ITeam>();

    private UnknownTeams() { }

    public IEnumerator<ITeam> GetEnumerator() => Teams.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public ITeams Add(ITeam team) => this;
    public void AddRange(IEnumerable<ITeam> teams) { }
    public TTeam Find<TTeam>() where TTeam : class, ITeam => UnknownTeam.Instance as TTeam;
}