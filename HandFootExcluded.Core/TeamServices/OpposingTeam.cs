using HandFootExcluded.Common;
using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.TeamServices;

public interface IOpposingTeam : ITeam<IOpposingPlayer, IOpposingPartner>
{
    new IOpposingPlayer Player { get; }
    new IOpposingPartner Partner { get; }
}

internal sealed partial class TeamBuilder : BuilderBase<TeamBuilder, ITeam>, ITeamBuilder, ITeamBuilderPartner, ITeamBuilderBuild
{
    private sealed class OpposingTeam : Team<IOpposingPlayer, IOpposingPartner>, IOpposingTeam
    {
        public override string Name => "Opposing Team";
        public OpposingTeam(IOpposingPlayer player, IOpposingPartner partner) : base(player, partner) { }
    }
}