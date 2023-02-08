using HandFootExcluded.Common;
using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.TeamServices;

public interface IStartingTeam : ITeam<IStartingPlayer, IStartingPartner>
{
    new IStartingPlayer Player { get; }
    new IStartingPartner Partner { get; }
}

internal sealed partial class TeamBuilder : BuilderBase<TeamBuilder, ITeam>, ITeamBuilder, ITeamBuilderPartner, ITeamBuilderBuild
{
    private sealed class StartingTeam : Team<IStartingPlayer, IStartingPartner>, IStartingTeam
    {
        public StartingTeam(IStartingPlayer player, IStartingPartner partner) : base(player, partner) { }
        public override string Name => "Starting Team";
    }
}