using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.Teamservices;

namespace HandFootExcluded.Core.RoundServices;

public sealed class UnknownRound : IRound
{
    public static readonly UnknownRound Instance = new();

    public int Order => -1;
    public int OpenAmount => 0;
    public IPlayers Players => UnknownPlayers.Instance;
    public ITeams Teams => UnknownTeams.Instance;

    private UnknownRound() { }
}