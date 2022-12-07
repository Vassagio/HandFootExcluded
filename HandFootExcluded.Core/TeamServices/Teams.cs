using System.Collections;
using HandFootExcluded.Common;
using HandFootExcluded.Core.TeamServices;

namespace HandFootExcluded.Core.Teamservices;

public interface ITeams : IEnumerable<ITeam>
{
    ITeams Add(ITeam team);
    void AddRange(IEnumerable<ITeam> teams);
    TTeam Find<TTeam>() where TTeam : class, ITeam;
}

internal sealed class Teams : ITeams
{
    private readonly ISet<ITeam> _teams = new HashSet<ITeam>();
    public int Count => _teams.Count;
    public bool IsReadOnly => _teams.IsReadOnly;

    public Teams() { }

    public Teams(ITeam team, params ITeam[] teams) : this()
    {
        Add(team);
        AddRange(teams);
    }

    public ITeams Add(ITeam team)
    {
        if (team is null or UnknownTeam) return this;

        _teams.Add(team);

        return this;
    }

    public void AddRange(IEnumerable<ITeam> teams)
    {
        foreach (var Team in teams.Where(p => p is not UnknownTeam))
            _teams.Add(Team);
    }

    public TTeam Find<TTeam>() where TTeam : class, ITeam =>
        _teams.OfType<TTeam>().SingleOrElse(UnknownTeam.Instance as TTeam);

    public IEnumerator<ITeam> GetEnumerator() => _teams.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}