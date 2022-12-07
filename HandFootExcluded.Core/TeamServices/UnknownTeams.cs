using System.Collections;
using HandFootExcluded.Core.TeamServices;

namespace HandFootExcluded.Core.Teamservices;

public sealed class UnknownTeams : ITeams
{
    public static readonly UnknownTeams Instance = new();
    private static readonly IEnumerable<ITeam> _teams = Enumerable.Empty<ITeam>();

    public IEnumerator<ITeam> GetEnumerator() => _teams.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public ITeams Add(ITeam team) => this;
    public void AddRange(IEnumerable<ITeam> teams) {  }
    public TTeam Find<TTeam>() where TTeam : class, ITeam => UnknownTeam.Instance as TTeam;

    private UnknownTeams() { }
}