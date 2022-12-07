using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.TeamServices;

public sealed class UnknownTeam : ITeam, ITeam<IPositionalPlayer, IPositionalPlayer>
{
    public static readonly UnknownTeam Instance = new();

    public IPositionalPlayer Player => UnknownPlayer.Instance;
    public IPositionalPlayer Partner => UnknownPlayer.Instance;

    private UnknownTeam() { }
}