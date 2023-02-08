using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.TeamServices;

public sealed class UnknownTeam : ITeam, ITeam<IPositionalPlayer, IPositionalPlayer>
{
    public static readonly UnknownTeam Instance = new();

    private UnknownTeam() { }

    public string Name => string.Empty;
    public IPlayers Players => UnknownPlayers.Instance;
    public IPositionalPlayer Player => UnknownPlayer.Instance;
    public IPositionalPlayer Partner => UnknownPlayer.Instance;
}