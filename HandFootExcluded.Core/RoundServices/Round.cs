using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.TeamServices;

namespace HandFootExcluded.Core.RoundServices;

public interface IRound
{
    int Order { get; }
    int OpenAmount { get; }
    IPlayers Players { get; }
    ITeams Teams { get; }
}

internal sealed record Round(int Order, int OpenAmount, IPlayers Players, ITeams Teams) : IRound
{
}