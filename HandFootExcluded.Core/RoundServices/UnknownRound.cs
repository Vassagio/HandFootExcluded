using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.TeamServices;

namespace HandFootExcluded.Core.RoundServices;

public sealed class UnknownRound : IRound
{
    public static readonly UnknownRound Instance = new();

    private UnknownRound() { }

    public int Order => -1;
    public int OpenAmount => 0;
    public IPlayers Players => UnknownPlayers.Instance;
    public ITeams Teams => UnknownTeams.Instance;
}